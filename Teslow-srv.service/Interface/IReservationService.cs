using Teslow_srv.Domain.Dto.Reservation;

namespace Teslow_srv.Service.Interface
{
    public interface IReservationService
    {
        Task<List<ReadReservationDto>> GetAllAsync(CancellationToken ct = default);

        Task<ReadReservationDto?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<ReadReservationDto> CreateAsync(CreateReservationDto dto, CancellationToken ct = default);

        Task<ReadReservationDto?> UpdateAsync(int id, UpdateReservationDto dto, CancellationToken ct = default);

        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
