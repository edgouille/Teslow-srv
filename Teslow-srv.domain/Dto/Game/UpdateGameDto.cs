using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Game
{
    public class UpdateGameDto
    {
        [MinLength(2)]
        [MaxLength(4)]
        public List<string>? Users { get; set; }

        public int? Score1 { get; set; }
        public int? Score2 { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? Date { get; set; }
    }
}
