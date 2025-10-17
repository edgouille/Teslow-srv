using System;

namespace Teslow_srv.Domain.Dto.Game
{
    public class ReadGameDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int DurationSeconds { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }
    }
}
