using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("GameTeams")]
    public class GameTeam
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid GameId { get; set; }

        public Game Game { get; set; } = null!;

        [Required]
        public int TeamNumber { get; set; }

        public ICollection<TeamPlayer> Players { get; set; } = new List<TeamPlayer>();
    }
}
