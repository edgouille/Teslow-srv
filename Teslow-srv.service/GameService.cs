using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<ReadGameDto>> GetAllAsync()
        {
            var games = await _db.Games
                .AsNoTracking()
                .Include(g => g.GameTables)
                .Include(g => g.GameTeams)
                .Include(g => g.Reservation)
                .ToListAsync();

            return games.Select(MapToReadDto);
        }

        public async Task<ReadGameDto?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var normalizedId = id.Trim();

            var game = await _db.Games
                .AsNoTracking()
                .Include(g => g.GameTables)
                .Include(g => g.GameTeams)
                .Include(g => g.Reservation)
                .FirstOrDefaultAsync(g => g.GameId == normalizedId);

            return game is null ? null : MapToReadDto(game);
        }

        public async Task<ReadGameDto> CreateAsync(CreateGameDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var gameId = string.IsNullOrWhiteSpace(dto.GameId) ? Guid.NewGuid().ToString("N") : dto.GameId.Trim();

            if (await _db.Games.AnyAsync(g => g.GameId == gameId))
            {
                throw new ArgumentException($"A game with id '{gameId}' already exists.");
            }

            var game = new Game
            {
                GameId = gameId,
                GameDate = dto.GameDate,
                GameDuration = dto.GameDuration,
                ScoreRed = dto.ScoreRed,
                ScoreBleu = dto.ScoreBleu
            };

            await ApplyTableAssignmentsAsync(game, dto.TableIds);
            await ApplyTeamAssignmentsAsync(game, dto.TeamIds);

            _db.Games.Add(game);
            await _db.SaveChangesAsync();

            return MapToReadDto(game);
        }

        public async Task<ReadGameDto?> UpdateAsync(string id, UpdateGameDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var normalizedId = id.Trim();

            var game = await _db.Games
                .Include(g => g.GameTables)
                .Include(g => g.GameTeams)
                .Include(g => g.Reservation)
                .FirstOrDefaultAsync(g => g.GameId == normalizedId);

            if (game is null)
            {
                return null;
            }

            if (dto.GameDate.HasValue)
            {
                game.GameDate = dto.GameDate.Value;
            }

            if (dto.GameDuration.HasValue)
            {
                game.GameDuration = dto.GameDuration.Value;
            }

            if (dto.ScoreRed.HasValue)
            {
                game.ScoreRed = dto.ScoreRed.Value;
            }

            if (dto.ScoreBleu.HasValue)
            {
                game.ScoreBleu = dto.ScoreBleu.Value;
            }

            if (dto.TableIds is not null)
            {
                await ApplyTableAssignmentsAsync(game, dto.TableIds);
            }

            if (dto.TeamIds is not null)
            {
                await ApplyTeamAssignmentsAsync(game, dto.TeamIds);
            }

            await _db.SaveChangesAsync();

            return MapToReadDto(game);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            var normalizedId = id.Trim();

            var game = await _db.Games.FirstOrDefaultAsync(g => g.GameId == normalizedId);
            if (game is null)
            {
                return false;
            }

            _db.Games.Remove(game);
            await _db.SaveChangesAsync();
            return true;
        }

        private async Task ApplyTableAssignmentsAsync(Game game, IEnumerable<string>? tableIds)
        {
            if (tableIds is null)
            {
                return;
            }

            var normalizedIds = tableIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingAssignments = game.GameTables.ToList();
            foreach (var assignment in existingAssignments.Where(a => !normalizedIds.Contains(a.GameTableId)))
            {
                game.GameTables.Remove(assignment);
            }

            if (normalizedIds.Count == 0)
            {
                return;
            }

            var tables = await _db.GameTables
                .Where(t => normalizedIds.Contains(t.GameTableId))
                .ToListAsync();

            if (tables.Count != normalizedIds.Count)
            {
                var found = tables.Select(t => t.GameTableId).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var missing = normalizedIds.Where(id => !found.Contains(id));
                throw new ArgumentException($"Unknown game table id(s): {string.Join(", ", missing)}");
            }

            var assigned = game.GameTables.Select(gt => gt.GameTableId).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var table in tables)
            {
                if (!assigned.Contains(table.GameTableId))
                {
                    game.GameTables.Add(new GameTableAssignment
                    {
                        GameId = game.GameId,
                        Game = game,
                        GameTableId = table.GameTableId,
                        GameTable = table
                    });
                }
            }
        }

        private async Task ApplyTeamAssignmentsAsync(Game game, IEnumerable<int>? teamIds)
        {
            if (teamIds is null)
            {
                return;
            }

            var normalizedIds = teamIds.Distinct().ToList();

            var existingAssignments = game.GameTeams.ToList();
            foreach (var assignment in existingAssignments.Where(a => !normalizedIds.Contains(a.TeamId)))
            {
                game.GameTeams.Remove(assignment);
            }

            if (normalizedIds.Count == 0)
            {
                return;
            }

            var teams = await _db.TeamMemberships
                .Where(t => normalizedIds.Contains(t.TeamId))
                .ToListAsync();

            if (teams.Count != normalizedIds.Count)
            {
                var found = teams.Select(t => t.TeamId).ToHashSet();
                var missing = normalizedIds.Where(id => !found.Contains(id));
                throw new ArgumentException($"Unknown team id(s): {string.Join(", ", missing)}");
            }

            var assigned = game.GameTeams.Select(gt => gt.TeamId).ToHashSet();
            foreach (var team in teams)
            {
                if (!assigned.Contains(team.TeamId))
                {
                    game.GameTeams.Add(new GameTeam
                    {
                        GameId = game.GameId,
                        Game = game,
                        TeamId = team.TeamId,
                        Team = team
                    });
                }
            }
        }

        private static ReadGameDto MapToReadDto(Game g) => new()
        {
            GameId = g.GameId,
            GameDate = g.GameDate,
            GameDuration = g.GameDuration,
            ScoreRed = g.ScoreRed,
            ScoreBleu = g.ScoreBleu,
            TableIds = g.GameTables.Select(gt => gt.GameTableId).ToList(),
            TeamIds = g.GameTeams.Select(gt => gt.TeamId).ToList(),
            ReservationId = g.Reservation?.ReservationId,
            ReservationStatus = g.Reservation?.Status
        };
    }
}
