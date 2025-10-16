using System;
using System.Collections.Generic;

namespace Teslow_srv.Domain.Dto.Game
{
    public class ReadGameDto
    {
        public string GameId { get; set; } = string.Empty;
        public DateTime GameDate { get; set; }
        public int GameDuration { get; set; }
        public int ScoreRed { get; set; }
        public int ScoreBleu { get; set; }
        public List<string> TableIds { get; set; } = new();
        public List<int> TeamIds { get; set; } = new();
        public int? ReservationId { get; set; }
        public string? ReservationStatus { get; set; }
    }
}
