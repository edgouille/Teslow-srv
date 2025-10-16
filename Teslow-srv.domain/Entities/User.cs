using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public required string UserName { get; set; }

        [Required]
        [MaxLength(256)]
        public required string PasswordHash { get; set; }

        [Required]
        [MaxLength(32)]
        public string Role { get; set; } = "User";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
