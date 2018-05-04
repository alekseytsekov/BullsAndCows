using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models.Dto.Game
{
    public class GameTurnDto
    {
        public int UserGuess { get; set; }
        public string CommentOnUserGuess { get; set; }
        public int AIGuess { get; set; }
        public string CommentOnAIGuess { get; set; }
        public int DigitToChange { get; set; }
    }
}
