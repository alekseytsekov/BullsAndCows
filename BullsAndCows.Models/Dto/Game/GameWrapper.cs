using System.Collections.Generic;

namespace BullsAndCows.Models.Dto.Game
{
    public class GameWrapper
    {
        public GameWrapper()
        {
            this.Games = new List<GameDto>();
        }

        public GameDto Game { get; set; }
        public IList<GameDto> Games { get; set; }
    }
}
