using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models.Domain.Game
{
    public class GameRanking
    {
        public int Id { get; set; }
        
        public long TimeSpanSeconds { get; set; }
        
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
