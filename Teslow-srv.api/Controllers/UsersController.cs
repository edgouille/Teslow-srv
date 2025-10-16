using Microsoft.AspNetCore.Mvc;
using Teslow_srv.Domain.Dto.User;
using Teslow_srv.Domain.Entities;
using Teslow_srv.service.User;

namespace Teslow_srv.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        //todo fix controller

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll()
        {
            
        }

        // GET: api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetUserDto>> GetOne(Guid id)
        {

        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<GetUserDto>> Create([FromBody] CreateUserDto dto)
        {
            
        }

        // PUT: api/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<GetUserDto>> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
           
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            
        }
    }
}
