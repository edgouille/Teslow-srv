using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teslow_srv.Domain.Entities
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Between 2 and 4 players
        [Required]
        [MinLength(2)]
        [MaxLength(4)]
        public List<string> Users { get; set; } = new();

        // Scores
        [Required]
        public int Score1 { get; set; }

        [Required]
        public int Score2 { get; set; }

        // Duration of the game (e.g., 00:30:00 for 30 minutes)
        [Required]
        public TimeSpan Duration { get; set; }

        // Date when the game took place
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
