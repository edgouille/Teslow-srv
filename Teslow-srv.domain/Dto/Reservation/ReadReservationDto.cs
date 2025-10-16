namespace Teslow_srv.Domain.Dto.Reservation
{
    public class ReadReservationDto
    {
        public int ReservationId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? GameId { get; set; }
    }
}
