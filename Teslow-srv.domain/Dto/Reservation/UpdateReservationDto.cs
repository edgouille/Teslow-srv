using System.ComponentModel.DataAnnotations;

namespace Teslow_srv.Domain.Dto.Reservation
{
    public class UpdateReservationDto
    {
        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(50)]
        public string? GameId { get; set; }
    }
}
