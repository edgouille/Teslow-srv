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

        // 🔹 Récupère tous les jeux
        public async Task<IEnumerable<ReadGameDto>> GetAllAsync()
        {
            var games = await _db.Games.AsNoTracking().ToListAsync();
            return games.Select(MapToReadDTO);
        }

        // 🔹 Récupère un jeu par Id
        public async Task<ReadGameDto?> GetByIdAsync(Guid id)
        {
            var game = await _db.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            return game is null ? null : MapToReadDTO(game);
        }

        // 🔹 Crée un nouveau jeu
        public async Task<ReadGameDto> CreateAsync(CreateGameDto dto)
        {
            // Validation métier (2 à 4 joueurs)
            var count = dto.Users?.Count ?? 0;
            if (count < 2 || count > 4)
                throw new ArgumentException("Number of users must be between 2 and 4.");

            var game = new Game
            {
                Users = dto.Users!,
                Score1 = dto.Score1,
                Score2 = dto.Score2,
                Duration = dto.Duration,
                Date = dto.Date
            };

            _db.Games.Add(game);
            await _db.SaveChangesAsync();

            return MapToReadDTO(game);
        }

        // 🔹 Met à jour un jeu existant
        public async Task<ReadGameDto?> UpdateAsync(Guid id, UpdateGameDto dto)
        {
            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game is null) return null;

            if (dto.Users is not null)
            {
                var count = dto.Users.Count;
                if (count < 2 || count > 4)
                    throw new ArgumentException("Number of users must be between 2 and 4.");
                game.Users = dto.Users;
            }

            if (dto.Score1.HasValue) game.Score1 = dto.Score1.Value;
            if (dto.Score2.HasValue) game.Score2 = dto.Score2.Value;
            if (dto.Duration.HasValue) game.Duration = dto.Duration.Value;
            if (dto.Date.HasValue) game.Date = dto.Date.Value;

            await _db.SaveChangesAsync();
            return MapToReadDTO(game);
        }

        // 🔹 Supprime un jeu
        public async Task<bool> DeleteAsync(Guid id)
        {
            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game is null) return false;

            _db.Games.Remove(game);
            await _db.SaveChangesAsync();
            return true;
        }

        // 🔹 Méthode privée de mapping
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
