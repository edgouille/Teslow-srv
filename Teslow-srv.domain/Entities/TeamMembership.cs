using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("TeamMemberships")]
    public class TeamMembership
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid TeamId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
