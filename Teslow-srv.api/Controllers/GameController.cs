using Microsoft.AspNetCore.Mvc;
using Teslow_srv.Domain.Dto.Game;
using Teslow_srv.Service.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Teslow_srv.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<ReadGameDto>> GetById(Guid id)
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<ReadGameDto>> Create(CreateGameDto dto)
        {
            var created = await _gameService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReadGameDto>> Update(Guid id, UpdateGameDto dto)
        {
            var updated = await _gameService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _gameService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
