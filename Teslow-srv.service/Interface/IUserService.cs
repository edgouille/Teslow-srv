using Teslow_srv.Domain.Dto.Auth;
using Teslow_srv.Domain.Dto.User;

namespace Teslow_srv.Service.Interface
{
    public interface IUserService
    {
        Task<List<GetUserDto>> GetAllAsync(CancellationToken ct = default);
        Task<GetUserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetUserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default);
        Task<GetUserDto?> UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<AuthenticatedUserDto?> ValidateCredentialsAsync(string userName, string password, CancellationToken ct = default);
        Task<List<UserLeaderboardDto>> GetLeaderboardAsync(CancellationToken ct = default);
    }
}
