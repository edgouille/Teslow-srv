using Teslow_srv.Domain.Dto.Auth;

namespace Teslow_srv.Api.Services
{
    public interface ITokenService
    {
        LoginResponseDto GenerateToken(AuthenticatedUserDto user);
    }
}
