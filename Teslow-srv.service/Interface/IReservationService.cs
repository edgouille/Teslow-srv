using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teslow_srv.Domain.Dto.Reservation;

namespace Teslow_srv.Service.Interface
{
    public interface IReservationService
    {
        Task<List<ReadReservationDto>> GetAllAsync(CancellationToken ct = default);

        Task<ReadReservationDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<ReadReservationDto> CreateAsync(CreateReservationDto dto, CancellationToken ct = default);

        Task<ReadReservationDto?> UpdateAsync(Guid id, UpdateReservationDto dto, CancellationToken ct = default);

        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
