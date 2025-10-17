using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
    }
}
