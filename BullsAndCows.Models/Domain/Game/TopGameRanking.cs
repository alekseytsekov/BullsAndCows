using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models.Domain.Game
{
    public class TopGameRanking
    {
        public int Id { get; set; }

        public int GameRankingId { get; set; }
        public virtual GameRanking GameRanking { get; set; }
    }
}
