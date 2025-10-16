using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
