using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BullsAndCows.Models.Dto.Game;

namespace BullsAndCows.Core
{
    public interface IGameManager
    {
        (bool, string) CreateGame(GameDto dto);
        GameDto GetGame(string userId, int gameId);
        IEnumerable<GameDto> GetGames(string userId);
        TurnResultDto ProcessTurn(string userId, int userGuess, int gameId);
    }
}
