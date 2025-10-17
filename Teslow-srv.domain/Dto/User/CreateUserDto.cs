using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.User
{
    public class CreateUserDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public required string UserName { get; set; }

        [MaxLength(100)]
        public string? DisplayName { get; set; }

        //[MaxLength(100)]
        //public string? CanonicalName { get; set; }

        [Range(0, 150)]
        public int? Age { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [MaxLength(32)]
        public string Role { get; set; } = "User";
    }
}
