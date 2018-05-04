using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using BullsAndCows.Core;
using BullsAndCows.Core.Utils;
using BullsAndCows.Globals;
using BullsAndCows.Globals.Extensions;
using BullsAndCows.Models.Dto.Game;

namespace BullsAndCows.Web.Controllers
{
    [Authorize]
    public class GameController : BaseController
    {
        private readonly IGameManager gameManager;
        private readonly IRankingManager rankingManager;

        public GameController(IGameManager gameManager, IRankingManager rankingManager)
        {
            this.gameManager = gameManager;
            this.rankingManager = rankingManager;
        }

        [HttpGet]
        [Route("game/new")]
        public ActionResult NewGame()
        {
            GameWrapper wrapper = new GameWrapper();
            GameDto dto = this.GetModelFromTempData<GameDto>(TDKey.GameDto);
            if (dto == null)
            {
                dto = new GameDto();
            }

            wrapper.Game = dto;

            return View(wrapper);
        }

        [HttpPost]
        [Route("game/new")]
        [ValidateAntiForgeryToken]
        public ActionResult NewGame(GameWrapper wrapper)
        {
            bool isValid;
            string message;

            (isValid, message) = this.IsGameValid(wrapper.Game);

            if (!isValid)
            {
                this.AddModelToTempData(TDKey.GameDto, wrapper.Game);
                this.ShowWarningMessage(message);

                return this.RedirectToAction(nameof(this.NewGame));
            }

            bool isSuccess;
            message = string.Empty;

            wrapper.Game.AIGuessedNumber = RandomGenerator.GetNumber();
            wrapper.Game.UserId = this.UserID;

            (isSuccess, message) = this.gameManager.CreateGame(wrapper.Game);
            if (!isSuccess)
            {
                this.AddModelToTempData(TDKey.GameDto, wrapper.Game);
                this.ShowWarningMessage(message);

                return this.RedirectToAction(nameof(this.NewGame));
            }

            this.ShowSuccessMessage("Successfully start new game!");

            this.AddModelToTempData(TDKey.CurrentGameId, int.Parse(message));

            return this.RedirectToAction(nameof(this.Play));
        }

        [HttpGet]
        public ActionResult Play(int? gameId)
        {
            int currentGameId = this.GetModelFromTempData<int>(TDKey.CurrentGameId);

            if (gameId != null && gameId > 0 && currentGameId < 1)
            {
                currentGameId = (int)gameId;
            }

            if (currentGameId < 1)
            {
                this.ShowWarningMessage("Invalid data! Something went wrong!");
                return this.RedirectToAction(nameof(this.NewGame).RemoveControllerSuffix());
            }

            GameDto dto = this.gameManager.GetGame(this.UserID, currentGameId);
            GameWrapper wrapper = new GameWrapper();
            wrapper.Game = dto;

            return this.View(wrapper);
        }

        [HttpPost]
        public ActionResult ProcessTurn(int userGuess, int gameId)
        {
            if (userGuess < 1000 || userGuess > 9999)
            {
                return this.Json(new{ Error = true }, JsonRequestBehavior.AllowGet);
            }

            TurnResultDto response = this.gameManager.ProcessTurn(this.UserID, userGuess, gameId);

            if (response.IsUserWinner)
            {
                this.rankingManager.CreateRanking(gameId);
            }
            
            return this.Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        [Route("game/history")]
        public ActionResult GameHistory()
        {
            IEnumerable<GameDto> games = this.gameManager.GetGames(this.UserID);
            GameWrapper wrapper = new GameWrapper();
            wrapper.Games.AddRange(games);

            return this.View(wrapper);
        }

        [HttpGet]
        [Authorize]
        [Route("game/details")]
        public ActionResult Details(int gameId)
        {
            GameDto dto = this.gameManager.GetGame(this.UserID, gameId);
            GameWrapper wrapper = new GameWrapper();
            wrapper.Game = dto;

            return this.View(wrapper);
        }

        private (bool, string) IsGameValid(GameDto dto)
        {
            if (dto.UserGuessedNumber < 1000 && dto.UserGuessedNumber > 9999)
            {
                return (false, "Guessed number should be between 1000 and 9999 inclusive!");
            }

            return (true, string.Empty);
        } 
    }
}