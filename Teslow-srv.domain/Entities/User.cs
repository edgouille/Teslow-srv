using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public required string UserName { get; set; }

        [MaxLength(50)]
        public string? Role { get; set; }

        [Required]
        [MaxLength(100)]
        public required string PasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();

        public ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
    }
}
