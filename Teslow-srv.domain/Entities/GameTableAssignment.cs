using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("GameTableAssignments")]
    public class GameTableAssignment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ReservationId { get; set; }

        public Reservation Reservation { get; set; } = null!;

        [Required]
        public Guid GameTableId { get; set; }

        public GameTable GameTable { get; set; } = null!;
    }
}
