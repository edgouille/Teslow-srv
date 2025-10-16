using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<ReadGameDto>>> GetAll()
        {
            return Ok(await _gameService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadGameDto>> GetById(string id)
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadGameDto>> Create([FromBody] CreateGameDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var created = await _gameService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.GameId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ReadGameDto>> Update(string id, [FromBody] UpdateGameDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var updated = await _gameService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _gameService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
