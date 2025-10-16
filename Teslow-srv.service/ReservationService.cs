using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Teslow_srv.Domain.Dto.Reservation;
using Teslow_srv.Domain.Entities;
using Teslow_srv.Infrastructure.Data;
using Teslow_srv.Service.Interface;

namespace Teslow_srv.Service
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _db;

        public ReservationService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ReadReservationDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Reservations
                .AsNoTracking()
                .OrderBy(r => r.ReservationId)
                .Select(r => new ReadReservationDto
                {
                    ReservationId = r.ReservationId,
                    Status = r.Status,
                    GameId = r.GameId
                })
                .ToListAsync(ct);
        }

        public async Task<ReadReservationDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var reservation = await _db.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ReservationId == id, ct);

            return reservation is null ? null : MapToReadDto(reservation);
        }

        public async Task<ReadReservationDto> CreateAsync(CreateReservationDto dto, CancellationToken ct = default)
        {
            var status = (dto.Status ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("Reservation status is required.");
            }

            var gameId = NormalizeGameId(dto.GameId);
            if (gameId is not null)
            {
                await EnsureGameExistsAsync(gameId, ct);
                await EnsureReservationSlotIsFreeAsync(gameId, null, ct);
            }

            var reservation = new Reservation
            {
                Status = status,
                GameId = gameId
            };

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync(ct);

            return MapToReadDto(reservation);
        }

        public async Task<ReadReservationDto?> UpdateAsync(int id, UpdateReservationDto dto, CancellationToken ct = default)
        {
            var reservation = await _db.Reservations.FirstOrDefaultAsync(r => r.ReservationId == id, ct);
            if (reservation is null)
            {
                return null;
            }

            if (dto.Status is not null)
            {
                var status = dto.Status.Trim();
                if (string.IsNullOrWhiteSpace(status))
                {
                    throw new ArgumentException("Reservation status cannot be empty.");
                }

                reservation.Status = status;
            }

            if (dto.GameId is not null)
            {
                var gameId = NormalizeGameId(dto.GameId);
                if (gameId is not null)
                {
                    await EnsureGameExistsAsync(gameId, ct);
                    await EnsureReservationSlotIsFreeAsync(gameId, reservation.ReservationId, ct);
                }

                reservation.GameId = gameId;
            }

            await _db.SaveChangesAsync(ct);

            return MapToReadDto(reservation);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var reservation = await _db.Reservations.FindAsync([id], ct);
            if (reservation is null)
            {
                return false;
            }

            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        private static string? NormalizeGameId(string? gameId)
        {
            if (string.IsNullOrWhiteSpace(gameId))
            {
                return null;
            }

            return gameId.Trim();
        }

        private async Task EnsureGameExistsAsync(string gameId, CancellationToken ct)
        {
            var exists = await _db.Games.AnyAsync(g => g.GameId == gameId, ct);
            if (!exists)
            {
                throw new ArgumentException($"Unknown game id: {gameId}");
            }
        }

        private async Task EnsureReservationSlotIsFreeAsync(string gameId, int? reservationId, CancellationToken ct)
        {
            var existing = await _db.Reservations
                .AnyAsync(r => r.GameId == gameId && r.ReservationId != reservationId, ct);

            if (existing)
            {
                throw new ArgumentException($"A reservation already exists for game '{gameId}'.");
            }
        }

        private static ReadReservationDto MapToReadDto(Reservation reservation) => new()
        {
            ReservationId = reservation.ReservationId,
            Status = reservation.Status,
            GameId = reservation.GameId
        };
    }
}
