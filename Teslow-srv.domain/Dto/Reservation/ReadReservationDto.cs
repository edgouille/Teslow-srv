using System;
using System.Collections.Generic;

namespace Teslow_srv.Domain.Dto.Reservation
{
    public class ReadReservationDto
    {
        public Guid Id { get; set; }

        public DateTime StartUtc { get; set; }

        public int DurationSeconds { get; set; }

        public byte Mode { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public List<Guid> TableIds { get; set; } = new();
    }
}
