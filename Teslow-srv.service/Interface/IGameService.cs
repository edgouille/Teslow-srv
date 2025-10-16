using System.Collections.Generic;
using System.Threading.Tasks;
using Teslow_srv.Domain.Dto.Game;

namespace Teslow_srv.Service.Interface
{
    public interface IGameService
    {
        Task<IEnumerable<ReadGameDto>> GetAllAsync();
        Task<ReadGameDto?> GetByIdAsync(string id);
        Task<ReadGameDto> CreateAsync(CreateGameDto dto);
        Task<ReadGameDto?> UpdateAsync(string id, UpdateGameDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
