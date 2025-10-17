using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Reservation
{
    public class CreateReservationDto
    {
        [Required]
        public DateTime StartUtc { get; set; }

        [Range(1, int.MaxValue)]
        public int DurationSeconds { get; set; }

        [Range(0, byte.MaxValue)]
        public byte Mode { get; set; }

        public List<Guid>? TableIds { get; set; }
    }
}
