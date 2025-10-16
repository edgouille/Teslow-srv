using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teslow_srv.Domain.Dto.Game
{
    public class ReadGameDto
    {
        public Guid Id { get; set; }
        public List<string> Users { get; set; } = new();
        public int Score1 { get; set; }
        public int Score2 { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Date { get; set; }
    }
}
