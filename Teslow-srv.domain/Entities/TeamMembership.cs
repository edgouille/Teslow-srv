using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("team_memberships")]
    public class TeamMembership
    {
        [Key]
        [Column("team_id")]
        public int TeamId { get; set; }

        [Column("team_color")]
        [StringLength(50)]
        public string TeamColor { get; set; } = string.Empty;

        [Column("player_goals")]
        public int PlayerGoals { get; set; }

        public ICollection<GameTeam> GameTeams { get; set; } = new List<GameTeam>();

        public ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
    }
}
