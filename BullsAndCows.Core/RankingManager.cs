using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BullsAndCows.Data.Repositories;
using BullsAndCows.Models.Domain.Game;
using BullsAndCows.Models.Dto.Game;

namespace BullsAndCows.Core
{
    public class RankingManager : IRankingManager
    {
        private const int MaxTopPlayers = 25;

        private readonly IRepository<Game> gameRepo;
        private readonly IRepository<GameRanking> gameRankingRepo;
        private readonly IRepository<TopGameRanking> topRankingRepo;

        public RankingManager(IRepository<Game> gameRepo, IRepository<GameRanking> gameRankingRepo, IRepository<TopGameRanking> topRankingRepo)
        {
            this.gameRepo = gameRepo;
            this.gameRankingRepo = gameRankingRepo;
            this.topRankingRepo = topRankingRepo;
        }

        public bool CreateRanking(int gameId)
        {
            var game = this.gameRepo.FirstOrDefault(x => x.Id == gameId);
            if (game == null)
            {
                return false;
            }

            if (!game.EndAt.HasValue)
            {
                return false;
            }

            var totalSeconds = Math.Ceiling((game.EndAt.Value - game.StartAt).TotalSeconds);
            var newGameRanking = new GameRanking()
            {
                GameId = game.Id,
                TimeSpanSeconds = (long)totalSeconds
            };

            this.gameRankingRepo.Add(newGameRanking);

            var allTopRankings = this.topRankingRepo.All();
            if (allTopRankings.Count() < MaxTopPlayers)
            {
                var newTopRanking = new TopGameRanking()
                {
                    GameRankingId = newGameRanking.Id
                };

                this.topRankingRepo.Add(newTopRanking);
            }
            else
            {
                TopGameRanking rankingToRemove = null;

                allTopRankings = allTopRankings.OrderBy(x => x.GameRanking.TimeSpanSeconds);
                foreach (var allTopRanking in allTopRankings)
                {
                    var playTimeInSeconds = allTopRanking.GameRanking.TimeSpanSeconds;

                    if (newGameRanking.TimeSpanSeconds < playTimeInSeconds)
                    {
                        rankingToRemove = allTopRanking;
                        break;
                    }
                }

                if (rankingToRemove != null)
                {
                    this.topRankingRepo.RemovePermanent(rankingToRemove);
                    var newTopRanking = new TopGameRanking()
                    {
                        GameRankingId = newGameRanking.Id
                    };

                    this.topRankingRepo.Add(newTopRanking);
                }
                else
                {
                    return false;
                }
                
            }

            return true;
        }

        public IEnumerable<GameRankingDto> GetTopRankings()
        {
            var entities = this.topRankingRepo.All();
            var dtos = this.MapEntityToDto(entities);

            return dtos;
        }

        public IEnumerable<GameRankingDto> GetPersonalRankings(string userId)
        {
            var entities = this.gameRankingRepo.Where(x => x.Game.UserId == userId);
            var dtos = this.MapEntityToDto(entities);

            return dtos;
        }

        private GameRankingDto MapEntityToDto(TopGameRanking entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = new GameRankingDto()
            {
                Id = entity.Id,
                Username = entity.GameRanking.Game.User.Email,
                StartAt = entity.GameRanking.Game.StartAt,
                EndAt = entity.GameRanking.Game.EndAt.Value,
                PlayTime = entity.GameRanking.TimeSpanSeconds,
                NumberOfTurns = entity.GameRanking.Game.GameTurns.Count
            };

            return dto;
        }

        private IEnumerable<GameRankingDto> MapEntityToDto(IEnumerable<TopGameRanking> entities)
        {
            if (entities == null || !entities.Any())
            {
                return Enumerable.Empty<GameRankingDto>();
            }

            var result = new List<GameRankingDto>();
            foreach (var entity in entities)
            {
                var dto = this.MapEntityToDto(entity);
                if (dto != null)
                {
                    result.Add(dto);
                }
            }

            return result;
        }

        private GameRankingDto MapEntityToDto(GameRanking entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = new GameRankingDto()
            {
                Id = entity.Id,
                Username = entity.Game.User.Email,
                StartAt = entity.Game.StartAt,
                EndAt = entity.Game.EndAt.Value,
                PlayTime = entity.TimeSpanSeconds,
                NumberOfTurns = entity.Game.GameTurns.Count
            };

            return dto;
        }

        private IEnumerable<GameRankingDto> MapEntityToDto(IEnumerable<GameRanking> entities)
        {
            if (entities == null || !entities.Any())
            {
                return Enumerable.Empty<GameRankingDto>();
            }

            var result = new List<GameRankingDto>();
            foreach (var entity in entities)
            {
                var dto = this.MapEntityToDto(entity);
                if (dto != null)
                {
                    result.Add(dto);
                }
            }

            return result;
        }
    }
}
