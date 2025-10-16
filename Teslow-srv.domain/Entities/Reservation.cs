using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("reservation")]
    public class Reservation
    {
        [Key]
        [Column("reservation_id")]
        public int ReservationId { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [Column("game_id")]
        [StringLength(50)]
        public string? GameId { get; set; }

        public Game? Game { get; set; }
    }
}
