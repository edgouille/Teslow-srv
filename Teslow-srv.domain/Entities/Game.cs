using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Entities
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Entre 2 et 4 joueurs
        [Required]
        [MinLength(2)]
        [MaxLength(4)]
        public required List<string> Users { get; set; }

        // Scores
        [Required]
        public int Score1 { get; set; }

        [Required]
        public int Score2 { get; set; }

        // Durée de la partie
        [Required]
        public TimeSpan Duration { get; set; }

        // Date de la partie
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
