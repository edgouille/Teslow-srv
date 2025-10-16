using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Game
{
    public class CreateGameDto
    {
        [StringLength(50)]
        public string? GameId { get; set; }

        [Required]
        public DateTime GameDate { get; set; } = DateTime.UtcNow.Date;

        [Required]
        public int GameDuration { get; set; }

        [Required]
        public int ScoreRed { get; set; }

        [Required]
        public int ScoreBleu { get; set; }

        public List<string>? TableIds { get; set; }

        public List<int>? TeamIds { get; set; }
    }
}
