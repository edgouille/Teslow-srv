using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("TeamPlayers")]
    public class TeamPlayer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid TeamId { get; set; }

        public GameTeam Team { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
