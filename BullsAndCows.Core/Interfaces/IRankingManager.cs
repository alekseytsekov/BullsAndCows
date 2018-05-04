using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BullsAndCows.Models.Dto.Game;

namespace BullsAndCows.Core
{
    public interface IRankingManager
    {
        bool CreateRanking(int gameId);
        IEnumerable<GameRankingDto> GetTopRankings();
        IEnumerable<GameRankingDto> GetPersonalRankings(string userId);
    }
}
