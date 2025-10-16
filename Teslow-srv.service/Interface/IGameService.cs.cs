using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teslow_srv.Domain.Dto.Game;

namespace Teslow_srv.Service.Interface
{
    public interface IGameService
    {
        Task<IEnumerable<ReadGameDto>> GetAllAsync();
        Task<ReadGameDto?> GetByIdAsync(Guid id);
        Task<ReadGameDto> CreateAsync(CreateGameDto dto);
        Task<ReadGameDto?> UpdateAsync(Guid id, UpdateGameDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
