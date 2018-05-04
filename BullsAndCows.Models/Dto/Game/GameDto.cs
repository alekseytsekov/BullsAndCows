using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models.Dto.Game
{
    public class GameDto
    {
        public GameDto()
        {
            this.GameTurns = new List<GameTurnDto>();
        }

        public int Id { get; set; }

        [Display(Name = "Start At")]
        public DateTime StartAt { get; set; }

        [Display(Name = "End At")]
        public DateTime? EndAt { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Guess a number")]
        [Required]
        [Range(1000, 10000)]
        public int UserGuessedNumber { get; set; }
        public int AIGuessedNumber { get; set; }

        public string CorrectDigits { get; set; }
        public string IncorrectDigits { get; set; }

        public IList<GameTurnDto> GameTurns { get; set; }
    }
}
