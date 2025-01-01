using AcopioAPIs.DTOs.User;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpGet]
        [ProducesResponseType<List<UserResultDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(string? name, string? userName, bool? estado)
        {
            var users = await _user.GetAll(name, userName, estado);
            return Ok(users);
        }
        [HttpGet("{userId}")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int userId)
        {
            var users = await _user.GetById(userId);
            return Ok(users);
        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] UserInsertDto insertDto)
        {
            var user = await _user.Insert(insertDto);
            return Ok(user);
        }
        [HttpPut]
        public async Task<ActionResult<UserResultDto>> UpdateUser([FromBody] UserUpdateDto updateDto)
        {
            var user = await _user.Update(updateDto);
            return Ok(user);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteUser([FromBody] UserDeleteDto deleteDto)
        {
            var result = await _user.Delete(deleteDto);
            return Ok(result);
        }
    }
}
