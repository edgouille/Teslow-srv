namespace Teslow_srv.Domain.Dto.Auth
{
    public class AuthenticatedUserDto
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public string Role { get; set; } = "User";
    }
}
