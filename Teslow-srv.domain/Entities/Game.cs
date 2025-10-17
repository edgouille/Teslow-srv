using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("Games")]
    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int Score1 { get; set; }

        [Required]
        public int Score2 { get; set; }

        [Required]
        public int DurationSeconds { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public ICollection<GameTeam> Teams { get; set; } = new List<GameTeam>();
    }
}
