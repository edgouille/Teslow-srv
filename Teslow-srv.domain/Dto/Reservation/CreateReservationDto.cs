using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Reservation
{
    public class CreateReservationDto
    {
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [StringLength(50)]
        public string? GameId { get; set; }
    }
}
