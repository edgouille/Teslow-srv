using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            var reservations = await _db.Reservations
                .AsNoTracking()
                .Include(r => r.TableAssignments)
                .OrderBy(r => r.StartUtc)
                .ToListAsync(ct);

            return reservations.Select(MapToReadDto).ToList();
        }

        public async Task<ReadReservationDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var reservation = await _db.Reservations
                .AsNoTracking()
                .Include(r => r.TableAssignments)
                .FirstOrDefaultAsync(r => r.Id == id, ct);

            return reservation is null ? null : MapToReadDto(reservation);
        }

        public async Task<ReadReservationDto> CreateAsync(CreateReservationDto dto, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.DurationSeconds <= 0)
            {
                throw new ArgumentException("Duration must be greater than zero.", nameof(dto));
            }

            var reservation = new Reservation
            {
                StartUtc = dto.StartUtc,
                DurationSeconds = dto.DurationSeconds,
                Mode = dto.Mode
            };

            await ApplyTableAssignmentsAsync(reservation, dto.TableIds, ct);

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync(ct);

            return MapToReadDto(reservation);
        }

        public async Task<ReadReservationDto?> UpdateAsync(Guid id, UpdateReservationDto dto, CancellationToken ct = default)
        {
            var reservation = await _db.Reservations
                .Include(r => r.TableAssignments)
                .FirstOrDefaultAsync(r => r.Id == id, ct);

            if (reservation is null)
            {
                return null;
            }

            if (dto.StartUtc.HasValue)
            {
                reservation.StartUtc = dto.StartUtc.Value;
            }

            if (dto.DurationSeconds.HasValue)
            {
                if (dto.DurationSeconds.Value <= 0)
                {
                    throw new ArgumentException("Duration must be greater than zero.", nameof(dto));
                }

                reservation.DurationSeconds = dto.DurationSeconds.Value;
            }

            if (dto.Mode.HasValue)
            {
                reservation.Mode = dto.Mode.Value;
            }

            if (dto.TableIds is not null)
            {
                await ApplyTableAssignmentsAsync(reservation, dto.TableIds, ct);
            }

            await _db.SaveChangesAsync(ct);

            return MapToReadDto(reservation);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var reservation = await _db.Reservations.FirstOrDefaultAsync(r => r.Id == id, ct);
            if (reservation is null)
            {
                return false;
            }

            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        private static ReadReservationDto MapToReadDto(Reservation reservation) => new()
        {
            Id = reservation.Id,
            StartUtc = reservation.StartUtc,
            DurationSeconds = reservation.DurationSeconds,
            Mode = reservation.Mode,
            CreatedAtUtc = reservation.CreatedAtUtc,
            TableIds = reservation.TableAssignments.Select(a => a.GameTableId).ToList()
        };

        private async Task ApplyTableAssignmentsAsync(Reservation reservation, IEnumerable<Guid>? tableIds, CancellationToken ct)
        {
            if (tableIds is null)
            {
                return;
            }

            var desiredIds = tableIds
                .Where(id => id != Guid.Empty)
                .Distinct()
                .ToList();

            if (desiredIds.Count == 0)
            {
                reservation.TableAssignments.Clear();
                return;
            }

            var tables = await _db.GameTables
                .Where(t => desiredIds.Contains(t.Id))
                .ToListAsync(ct);

            if (tables.Count != desiredIds.Count)
            {
                var found = tables.Select(t => t.Id).ToHashSet();
                var missing = desiredIds.Where(id => !found.Contains(id));
                throw new ArgumentException($"Unknown game table id(s): {string.Join(", ", missing)}");
            }

            var currentAssignments = reservation.TableAssignments
                .Where(a => !desiredIds.Contains(a.GameTableId))
                .ToList();

            foreach (var assignment in currentAssignments)
            {
                reservation.TableAssignments.Remove(assignment);
            }

            var existingIds = reservation.TableAssignments
                .Select(a => a.GameTableId)
                .ToHashSet();

            foreach (var table in tables)
            {
                if (existingIds.Add(table.Id))
                {
                    reservation.TableAssignments.Add(new GameTableAssignment
                    {
                        Reservation = reservation,
                        ReservationId = reservation.Id,
                        GameTable = table,
                        GameTableId = table.Id
                    });
                }
            }
        }
    }
}
