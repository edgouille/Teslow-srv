using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.User
{
    public class UpdateUserDto
    {
        [MinLength(3)]
        [MaxLength(100)]
        public string? UserName { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }

        [MaxLength(32)]
        public string? Role { get; set; }
    }
}
