using System;
using System.Collections.Generic;

namespace Teslow_srv.Domain.Dto.Game
{
    public class UpdateGameDto
    {
        public DateTime? GameDate { get; set; }
        public int? GameDuration { get; set; }
        public int? ScoreRed { get; set; }
        public int? ScoreBleu { get; set; }
        public List<string>? TableIds { get; set; }
        public List<int>? TeamIds { get; set; }
    }
}
