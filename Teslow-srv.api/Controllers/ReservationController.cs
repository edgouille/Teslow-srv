using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teslow_srv.Domain.Dto.Reservation;
using Teslow_srv.Service.Interface;

namespace Teslow_srv.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ReadReservationDto>>> GetAll(CancellationToken ct)
        {
            var reservations = await _reservationService.GetAllAsync(ct);
            return Ok(reservations);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadReservationDto>> GetById(int id, CancellationToken ct)
        {
            var reservation = await _reservationService.GetByIdAsync(id, ct);
            return reservation is null ? NotFound() : Ok(reservation);
        }

        [HttpPost]
        public async Task<ActionResult<ReadReservationDto>> Create([FromBody] CreateReservationDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var created = await _reservationService.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id = created.ReservationId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadReservationDto>> Update(int id, [FromBody] UpdateReservationDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var updated = await _reservationService.UpdateAsync(id, dto, ct);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _reservationService.DeleteAsync(id, ct);
            return deleted ? NoContent() : NotFound();
        }
    }
}
