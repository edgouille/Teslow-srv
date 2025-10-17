using Microsoft.EntityFrameworkCore;
using Teslow_srv.Domain.Dto.Game;
using Teslow_srv.Domain.Entities;
using Teslow_srv.Infrastructure.Data;
using Teslow_srv.Service.Interface;

namespace Teslow_srv.Service
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _db;

        public GameService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ReadGameDto>> GetAllAsync(CancellationToken ct = default)
        {
            var games = await _db.Games
                .AsNoTracking()
                .OrderByDescending(g => g.Date)
                .ToListAsync(ct);

            return games.Select(MapToReadDto);
        }

        public async Task<ReadGameDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var game = await _db.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id, ct);

            return game is null ? null : MapToReadDto(game);
        }

        public async Task<ReadGameDto> CreateAsync(CreateGameDto dto, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            ValidateScores(dto.Score1, dto.Score2);
            if (dto.DurationSeconds < 0)
            {
                throw new ArgumentException("Duration must be greater than or equal to zero.", nameof(dto));
            }

            var game = new Game
            {
                Date = dto.Date,
                DurationSeconds = dto.DurationSeconds,
                Score1 = dto.Score1,
                Score2 = dto.Score2
            };

            _db.Games.Add(game);
            await _db.SaveChangesAsync(ct);

            return MapToReadDto(game);
        }

        public async Task<ReadGameDto?> UpdateAsync(Guid id, UpdateGameDto dto, CancellationToken ct = default)
        {
            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == id, ct);
            if (game is null)
            {
                return null;
            }

            if (dto.Date.HasValue)
            {
                game.Date = dto.Date.Value;
            }

            if (dto.DurationSeconds.HasValue)
            {
                if (dto.DurationSeconds.Value < 0)
                {
                    throw new ArgumentException("Duration must be greater than or equal to zero.", nameof(dto));
                }

                game.DurationSeconds = dto.DurationSeconds.Value;
            }

            if (dto.Score1.HasValue || dto.Score2.HasValue)
            {
                ValidateScores(dto.Score1 ?? game.Score1, dto.Score2 ?? game.Score2);
            }

            if (dto.Score1.HasValue)
            {
                game.Score1 = dto.Score1.Value;
            }

            if (dto.Score2.HasValue)
            {
                game.Score2 = dto.Score2.Value;
            }

            await _db.SaveChangesAsync(ct);

            return MapToReadDto(game);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == id, ct);
            if (game is null)
            {
                return false;
            }

            _db.Games.Remove(game);
            await _db.SaveChangesAsync(ct);
            return true;
        }
        public async Task<ReadGameDto> AddScoreAsync(AddScoreGameDto dto, CancellationToken ct = default)
        {
            if (dto.Team1 < 0 || dto.Team2 < 0)
                throw new ArgumentException("Les scores doivent être positifs.");

            if (dto.Duration < 0)
                throw new ArgumentException("La durée doit être positive.");

            var game = new Game
            {
                Score1 = dto.Team1,
                Score2 = dto.Team2,
                DurationSeconds = dto.Duration,
                Date = DateTime.UtcNow
            };

            _db.Games.Add(game);
            await _db.SaveChangesAsync(ct);

            return new ReadGameDto
            {
                Id = game.Id,
                Score1 = game.Score1,
                Score2 = game.Score2,
                DurationSeconds = game.DurationSeconds,
                Date = game.Date
            };
        }

        private static void ValidateScores(int score1, int score2)
        {
            if (score1 < 0 || score2 < 0)
            {
                throw new ArgumentException("Scores must be greater than or equal to zero.");
            }
        }

        private static ReadGameDto MapToReadDto(Game game) => new()
        {
            Id = game.Id,
            Date = game.Date,
            DurationSeconds = game.DurationSeconds,
            Score1 = game.Score1,
            Score2 = game.Score2
        };
    }
}
