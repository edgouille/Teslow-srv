namespace Teslow_srv.Domain.Dto.Auth
{
    public class LoginResponseDto
    {
        public required string Token { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
