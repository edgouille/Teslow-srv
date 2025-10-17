using System;
using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Game
{
    public class CreateGameDto
    {
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Range(0, int.MaxValue)]
        public int DurationSeconds { get; set; }

        [Range(0, int.MaxValue)]
        public int Score1 { get; set; }

        [Range(0, int.MaxValue)]
        public int Score2 { get; set; }
    }
}
