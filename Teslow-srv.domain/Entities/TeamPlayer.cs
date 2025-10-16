using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("teams_players")]
    public class TeamPlayer
    {
        [Column("player_id")]
        public string PlayerId { get; set; } = null!;

        public Player Player { get; set; } = null!;

        [Column("team_id")]
        public int TeamId { get; set; }

        public TeamMembership Team { get; set; } = null!;
    }
}
