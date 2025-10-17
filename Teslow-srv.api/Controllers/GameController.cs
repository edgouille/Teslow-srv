using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teslow_srv.Domain.Dto.Game;
using Teslow_srv.Service.Interface;

namespace Teslow_srv.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadGameDto>>> GetAll(CancellationToken ct)
        {
            return Ok(await _gameService.GetAllAsync(ct));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ReadGameDto>> GetById(Guid id, CancellationToken ct)
        {
            var game = await _gameService.GetByIdAsync(id, ct);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadGameDto>> Create([FromBody] CreateGameDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var created = await _gameService.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadGameDto>> Update(Guid id, [FromBody] UpdateGameDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var updated = await _gameService.UpdateAsync(id, dto, ct);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _gameService.DeleteAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost("addScore")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> AddScore([FromBody] AddScoreGameDto scores, CancellationToken ct)
        {
            if (scores is null) return BadRequest("Payload required.");

            try
            {
                var created = await _gameService.AddScoreAsync(scores, ct);
                // 201 avec l’objet créé
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
