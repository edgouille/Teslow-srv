using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("games_teams")]
    public class GameTeam
    {
        [Column("game_id")]
        public string GameId { get; set; } = null!;

        public Game Game { get; set; } = null!;

        [Column("team_id")]
        public int TeamId { get; set; }

        public TeamMembership Team { get; set; } = null!;
    }
}
