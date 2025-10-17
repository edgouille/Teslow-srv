using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teslow_srv.Domain.Dto.Game;

namespace Teslow_srv.Service.Interface
{
    public interface IGameService
    {
        Task<IEnumerable<ReadGameDto>> GetAllAsync(CancellationToken ct = default);
        Task<ReadGameDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<ReadGameDto> CreateAsync(CreateGameDto dto, CancellationToken ct = default);
        Task<ReadGameDto?> UpdateAsync(Guid id, UpdateGameDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<ReadGameDto> AddScoreAsync(AddScoreGameDto dto, CancellationToken ct = default);
    }
}
