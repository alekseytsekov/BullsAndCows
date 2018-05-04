using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models.Dto.Game
{
    public class RankingWrapper
    {
        public RankingWrapper()
        {
            this.Rankings = new List<GameRankingDto>();
        }

        public bool IsTopRankings { get; set; }
        public IList<GameRankingDto> Rankings { get; set; }
    }
}
