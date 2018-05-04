using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models.Dto.Game
{
    public class TurnResultDto
    {
        public int UserGuess { get; set; }
        public string CommentOnUserGuess { get; set; }
        public int AiGuess { get; set; }
        public string CommentOnAiGuess { get; set; }
        public bool HasWinner { get; set; }
        public bool IsUserWinner { get; set; }
    }
}
