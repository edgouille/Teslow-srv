using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Game
{
    public class CreateGameDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "At least 2 players required")]
        [MaxLength(4, ErrorMessage = "Maximum 4 players allowed")]
        public List<string> Users { get; set; } = new();

        [Required]
        public int Score1 { get; set; }

        [Required]
        public int Score2 { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
