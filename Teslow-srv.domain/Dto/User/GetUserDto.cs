namespace Teslow_srv.Domain.Dto.User
{
    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
