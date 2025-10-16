using Teslow_srv.Service.Game.Interface;
using Teslow_srv.Domain.Dto.Game;
using Teslow_srv.Domain.Entities;

namespace Teslow_srv.Service.Game
{
    public class GameService : IGameService
    {
        private readonly List<Game> _games = new();// db context 

        public Task<IEnumerable<ReadGameDto>> GetAllAsync()
        {
            var result = _games.Select(MapToReadDTO);
            return Task.FromResult(result);
        }

        public Task<ReadGameDto?> GetByIdAsync(Guid id)
        {
            var game = _games.FirstOrDefault(g => g.Id == id);
            return Task.FromResult(game is null ? null : MapToReadDTO(game));
        }

        public Task<ReadGameDto> CreateAsync(CreateGameDto dto)
        {
            if (dto.Users.Count < 2 || dto.Users.Count > 4)
                throw new ArgumentException("Number of users must be between 2 and 4.");

            var game = new Game
            {
                Users = dto.Users,
                Score1 = dto.Score1,
                Score2 = dto.Score2,
                Duration = dto.Duration,
                Date = dto.Date
            };

            _games.Add(game);
            return Task.FromResult(MapToReadDTO(game));
        }

        public Task<ReadGameDto?> UpdateAsync(Guid id, UpdateGameDto dto)
        {
            var game = _games.FirstOrDefault(g => g.Id == id);
            if (game == null) return Task.FromResult<ReadGameDto?>(null);

            if (dto.Users != null) game.Users = dto.Users;
            if (dto.Score1.HasValue) game.Score1 = dto.Score1.Value;
            if (dto.Score2.HasValue) game.Score2 = dto.Score2.Value;
            if (dto.Duration.HasValue) game.Duration = dto.Duration.Value;
            if (dto.Date.HasValue) game.Date = dto.Date.Value;

            return Task.FromResult(MapToReadDTO(game));
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var game = _games.FirstOrDefault(g => g.Id == id);
            if (game == null) return Task.FromResult(false);

            _games.Remove(game);
            return Task.FromResult(true);
        }

        private static ReadGameDto MapToReadDTO(Game g) => new()
        {
            Id = g.Id,
            Users = g.Users,
            Score1 = g.Score1,
            Score2 = g.Score2,
            Duration = g.Duration,
            Date = g.Date
        };
    }
}
