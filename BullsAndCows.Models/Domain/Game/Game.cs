using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models.Domain.Game
{
    public class Game
    {
        public Game()
        {
            this.GameTurns = new List<GameTurn>();
        }

        public int Id { get; set; }

        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        
        public int UserGuessedNumber { get; set; }
        public int AIGuessedNumber { get; set; }
        
        public string Bulls { get; set; }

        public virtual IList<GameTurn> GameTurns { get; set; }
    }
}
