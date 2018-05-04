using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BullsAndCows.Core.Utils;
using BullsAndCows.Data.Repositories;
using BullsAndCows.Models.Domain.Game;
using BullsAndCows.Models.Dto.Game;

namespace BullsAndCows.Core
{
    public class GameManager : IGameManager
    {
        private readonly IRepository<Game> gameRepo;

        public GameManager(IRepository<Game> gameRepo)
        {
            this.gameRepo = gameRepo;
        }

        public (bool, string) CreateGame(GameDto dto)
        {
            dto.StartAt = DateTime.UtcNow;
            
            Game entity = this.MapDtoToEntity(dto);

            try
            {
                gameRepo.Add(entity);
            }
            catch (Exception e)
            {
                return (false, "Something went wrong! Invalid data!");
            }

            return (true, entity.Id.ToString());
        }


        public GameDto GetGame(string userId, int gameId)
        {
            Game entity = this.gameRepo.FirstOrDefault(x => x.Id == gameId && x.UserId == userId);
            if (entity == null)
            {
                return null;
            }

            GameDto dto = this.MapEntityToDto(entity);

            return dto;
        }

        public IEnumerable<GameDto> GetGames(string userId)
        {
            var entities = this.gameRepo.Where(x => x.UserId == userId).ToList();
            
            IEnumerable<GameDto> dtos = this.MapEntityToDto(entities);

            return dtos;
        }


        public TurnResultDto ProcessTurn(string userId, int userGuess, int gameId)
        {
            Game game = gameRepo.FirstOrDefault(x => x.Id == gameId && x.UserId == userId);
            if (game == null)
            {
                return null;
            }

            bool hasWinner = false;
            bool isFirstTurn = false;
            
            int aiGuess = 0;
            
            int index = 3;
            bool indexIsChanged = false;
            int unmodifiedIndex = 0;

            int newAiBulls = 0;
            int newAiCows = 0;
            
            int oldNum = 0;
            int newNum = 0;

            if (game.GameTurns.Any())
            {
                var lastTurn = game.GameTurns.OrderByDescending(x => x.Id).Skip(0).Take(1).FirstOrDefault();
                if (lastTurn != null)
                {
                    aiGuess = lastTurn.AIGuess;
                    index = lastTurn.DigitToChange;
                }
            }
            else
            {
                aiGuess = RandomGenerator.GetNumber();
                isFirstTurn = true;
            }

            Dictionary<int,int> bullCollection = this.ParseBullPosition(game.Bulls);
            
            int oldAiBulls = this.GetBulls(aiGuess, game.UserGuessedNumber);
            int oldAiCows = this.GetCows(aiGuess, game.UserGuessedNumber);

            
            if (!isFirstTurn)
            {
                var asArr = ConvertToDigits(aiGuess);

                oldNum = asArr[index];
                newNum = oldNum + 1;
                if (newNum > 9)
                {
                    newNum = 0;

                }

                asArr[index] = newNum;

                aiGuess = ArrToInt(asArr);

                newAiBulls = GetBulls(aiGuess, game.UserGuessedNumber);
                newAiCows = GetCows(aiGuess, game.UserGuessedNumber);

                if (newAiBulls == 4)
                {
                    hasWinner = true;
                }

                if (newAiBulls + newAiCows > oldAiBulls + oldAiCows && !hasWinner)
                {
                    unmodifiedIndex = index;

                    index = this.GetIndex(index, bullCollection);

                    indexIsChanged = true;
                }

                if (newAiBulls > oldAiBulls && !hasWinner)
                {
                    int correctIndex = index;
                    if (indexIsChanged)
                    {
                        correctIndex = unmodifiedIndex;
                    }
                    
                    bullCollection.Add(correctIndex, newNum);
                    game.Bulls += $"{correctIndex}x{newNum};";

                    if (!indexIsChanged)
                    {
                        index = this.GetIndex(index, bullCollection);
                    }
                }
                else if (newAiBulls < oldAiBulls && !hasWinner)
                {
                    int correctIndex = index;
                    if (indexIsChanged)
                    {
                        correctIndex = unmodifiedIndex;
                    }

                    bullCollection.Add(correctIndex, oldNum);
                    game.Bulls += $"{correctIndex}x{oldNum};";
                    //correctNums.Add(newNum);

                    asArr[index] = oldNum;
                    aiGuess = ArrToInt(asArr);

                    if (!indexIsChanged)
                    {
                        index = this.GetIndex(index, bullCollection);
                    }
                }
                
                oldAiBulls = newAiBulls;
                oldAiCows = newAiCows;
            }
            
            int userBulls = this.GetBulls(userGuess, game.AIGuessedNumber);
            int userCows = this.GetCows(userGuess, game.AIGuessedNumber);

            string userComment = $"Bulls: {userBulls}, Cows: {userCows}";
            string aiComment = $"Bulls: {oldAiBulls}, Cows: {oldAiCows}";

            var newTurn = new GameTurn()
            {
                GameId = game.Id,
                UserGuess = userGuess,
                AIGuess = aiGuess,
                CommentOnUserGuess = userComment,
                CommentOnAIGuess = aiComment,
                DigitToChange = index
            };

            game.GameTurns.Add(newTurn);

            bool isUserWinner = false;

            if (oldAiBulls == 4)
            {
                hasWinner = true;
                game.EndAt = DateTime.UtcNow;
            }
            else if (userBulls == 4)
            {
                hasWinner = true;
                isUserWinner = true;
                game.EndAt = DateTime.UtcNow;
            }

            gameRepo.Update();
            
            var result = new TurnResultDto()
            {
                UserGuess = userGuess,
                CommentOnUserGuess = userComment,
                AiGuess = aiGuess,
                CommentOnAiGuess = aiComment,
                HasWinner = hasWinner,
                IsUserWinner = isUserWinner
            };

            return result;
        }


        private int GetIndex(int index, IDictionary<int,int> bullCollection)
        {
            index--;
            if (index < 0)
            {
                index = 3;
            }

            while (bullCollection.ContainsKey(index))
            {
                index--;
                if (index < 0)
                {
                    index = 3;
                }
            }

            return index;
        }

        private int ArrToInt(int[] arr)
        {
            int result = 0;
            int multipicator = 1;

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                result += arr[i] * multipicator;
                multipicator *= 10;
            }

            return result;
        }

        private Dictionary<int, int> ParseBullPosition(string str)
        {
            var bullsIndexDigit = new Dictionary<int,int>();

            if (string.IsNullOrWhiteSpace(str))
            {
                return bullsIndexDigit;
            }

            var tokens = str.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var kvp in tokens)
            {
                var kvpTokens = kvp.Split(new[] {'x'}, StringSplitOptions.RemoveEmptyEntries);
                if (kvpTokens.Length == 2)
                {
                    int index = 0;
                    int digit = 0;

                    if (int.TryParse(kvpTokens[0], out index) && int.TryParse(kvpTokens[1], out digit))
                    {
                        if (!bullsIndexDigit.ContainsKey(index))
                        {
                            bullsIndexDigit.Add(index, digit);
                        }
                    }
                }
            }

            return bullsIndexDigit;
        }
        

        private int GetBulls(int guess, int number)
        {
            int[] guessAsDigits = this.ConvertToDigits(guess);
            int[] numberAsDigits = this.ConvertToDigits(number);

            int bulls = 0;

            for (int i = 0; i < 4; i++)
            {
                if (guessAsDigits[i] == numberAsDigits[i])
                {
                    bulls++;
                }
            }

            return bulls;
        }

        public int GetCows(int guess, int number)
        {
            int[] guessAsDigits = this.ConvertToDigits(guess);
            int[] numberAsDigits = this.ConvertToDigits(number);

            int cows = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (guessAsDigits[i] == numberAsDigits[j])
                    {
                        cows++;
                        guessAsDigits[i] = -1;
                        numberAsDigits[j] = -2;
                    }
                }
            }

            return cows;
        }

        public int[] ConvertToDigits(int number)
        {
            int[] temp = new int[4];

            for (int i = 3; i >= 0; i--)
            {
                temp[i] = number % 10;
                number /= 10;
            }

            return temp;
        }

        private Game MapDtoToEntity(GameDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new Game()
            {
                UserId = dto.UserId,
                UserGuessedNumber = dto.UserGuessedNumber,
                AIGuessedNumber = dto.AIGuessedNumber,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt
            };

            return entity;
        }

        private GameDto MapEntityToDto(Game entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = new GameDto()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                UserGuessedNumber = entity.UserGuessedNumber,
                AIGuessedNumber = entity.AIGuessedNumber,
                StartAt = entity.StartAt,
                EndAt = entity.EndAt
            };

            foreach (var gameTurn in entity.GameTurns)
            {
                GameTurnDto gameTurnDto = new GameTurnDto()
                {
                    CommentOnUserGuess = gameTurn.CommentOnUserGuess,
                    UserGuess = gameTurn.UserGuess,
                    CommentOnAIGuess = gameTurn.CommentOnAIGuess,
                    DigitToChange = gameTurn.DigitToChange,
                    AIGuess = gameTurn.AIGuess,
                };

                dto.GameTurns.Add(gameTurnDto);
            }

            return dto;
        }

        private IEnumerable<GameDto> MapEntityToDto(IEnumerable<Game> entities)
        {
            if (entities == null || !entities.Any())
            {
                return Enumerable.Empty<GameDto>();
            }

            var result = new List<GameDto>();
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
