using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BullsAndCows.Core;
using BullsAndCows.Globals.Extensions;
using BullsAndCows.Models.Dto.Game;

namespace BullsAndCows.Web.Controllers
{
    public class RankingController : BaseController
    {
        private readonly IRankingManager rankingManager;

        public RankingController(IRankingManager rankingManager)
        {
            this.rankingManager = rankingManager;
        }

        // GET: Ranking
        [HttpGet]
        [Route("game/ranking/top")]
        public ActionResult TopRankings()
        {
            var dtos = this.rankingManager.GetTopRankings()
                                            .OrderBy(x => x.PlayTime)
                                            .ThenBy(x => x.NumberOfTurns);

            var wrapper = new RankingWrapper();
            wrapper.IsTopRankings = true;
            wrapper.Rankings.AddRange(dtos);

            return this.View("ListRankings", wrapper);
        }

        [HttpGet]
        [Authorize]
        [Route("game/ranking/personal")]
        public ActionResult PersonalRankings()
        {
            var dtos = this.rankingManager.GetPersonalRankings(this.UserID)
                                            .OrderBy(x => x.PlayTime)
                                            .ThenBy(x => x.NumberOfTurns);

            var wrapper = new RankingWrapper();
            wrapper.Rankings.AddRange(dtos);

            return this.View("ListRankings", wrapper);
        }
    }
}