using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("players")]
    public class Player
    {
        [Key]
        [Column("player_id")]
        [StringLength(50)]
        public string PlayerId { get; set; } = Guid.NewGuid().ToString("N");

        [Column("player_name")]
        [StringLength(50)]
        public string PlayerName { get; set; } = string.Empty;

        [Column("player_canonical_name")]
        [StringLength(100)]
        public string PlayerCanonicalName { get; set; } = string.Empty;

        [Column("player_age")]
        public int PlayerAge { get; set; }

        public ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
    }
}
