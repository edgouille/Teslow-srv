using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("games")]
    public class Game
    {
        [Key]
        [Column("game_id")]
        [StringLength(50)]
        public string GameId { get; set; } = Guid.NewGuid().ToString("N");

        [Column("game_date")]
        public DateTime GameDate { get; set; } = DateTime.UtcNow.Date;

        [Column("game_duration")]
        public int GameDuration { get; set; }

        [Column("score_red")]
        public int ScoreRed { get; set; }

        [Column("score_bleu")]
        public int ScoreBleu { get; set; }

        public ICollection<GameTableAssignment> GameTables { get; set; } = new List<GameTableAssignment>();

        public ICollection<GameTeam> GameTeams { get; set; } = new List<GameTeam>();

        public Reservation? Reservation { get; set; }
    }
}
