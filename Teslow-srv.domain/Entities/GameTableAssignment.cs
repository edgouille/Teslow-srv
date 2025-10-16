using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("games_tables")]
    public class GameTableAssignment
    {
        [Column("game_id")]
        public string GameId { get; set; } = null!;

        public Game Game { get; set; } = null!;

        [Column("game_table_id")]
        public string GameTableId { get; set; } = null!;

        public GameTable GameTable { get; set; } = null!;
    }
}
