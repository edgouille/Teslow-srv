using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("teams_players")]
    public class TeamPlayer
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        [Column("team_id")]
        public int TeamId { get; set; }

        public TeamMembership Team { get; set; } = null!;
    }
}
