using Microsoft.AspNetCore.Mvc;
using Teslow_srv.domain.Dto.User;
using Teslow_srv.domain.Entities;

namespace Teslow_srv.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll()
        {
            var users = await _db.Users
                .AsNoTracking()
                .Select(u => u.ToGetDto())
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetUserDto>> GetOne(Guid id)
        {
            var u = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (u is null) return NotFound();

            return Ok(u.ToGetDto());
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<GetUserDto>> Create([FromBody] CreateUserDto dto)
        {
            if (dto is null) return BadRequest();

            // Si le client ne fournit pas d'Id, on en génère un.
            var id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;

            // (Optionnel) empêcher doublons sur l'Id si le client le fournit
            var exists = await _db.Users.AnyAsync(x => x.Id == id);
            if (exists) return Conflict("Un utilisateur avec cet Id existe déjà.");

            var user = new User
            {
                Id = id,
                UserName = (dto.UserName ?? string.Empty).Trim()
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var read = user.ToGetDto();//todo : ??? idk fix
            return CreatedAtAction(nameof(GetOne), new { id = read.Id }, read);
        }

        // PUT: api/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<GetUserDto>> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null) return NotFound();

            user.ApplyUpdate(dto);
            await _db.SaveChangesAsync();

            return Ok(user.ToGetDto());
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null) return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
