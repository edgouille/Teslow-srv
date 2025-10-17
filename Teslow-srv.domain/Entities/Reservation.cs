using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime StartUtc { get; set; }

        [Required]
        public int DurationSeconds { get; set; }

        [Required]
        public byte Mode { get; set; }

        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public ICollection<GameTableAssignment> TableAssignments { get; set; } = new List<GameTableAssignment>();
    }
}
