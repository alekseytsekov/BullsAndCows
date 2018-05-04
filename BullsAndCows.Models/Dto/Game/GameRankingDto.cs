using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Models.Dto.Game
{
    public class GameRankingDto
    {
        public int Id { get; set; }

        [Display(Name = "Nickname")]
        public string Username { get; set; }

        [Display(Name = "Play Time")]
        public long PlayTime { get; set; }

        [Display(Name = "Start At")]
        public DateTime StartAt { get; set; }

        [Display(Name = "End At")]
        public DateTime EndAt { get; set; }

        [Display(Name = "Number Of Turns")]
        public int NumberOfTurns { get; set; }
    }
}
