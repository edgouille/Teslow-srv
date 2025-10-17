namespace Teslow_srv.Domain.Dto.User
{
    public class GetUserDto
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? CanonicalName { get; set; }
        public int? Age { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; }
    }
}
