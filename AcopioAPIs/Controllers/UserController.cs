using AcopioAPIs.DTOs.User;
using AcopioAPIs.Repositories;
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
        public async Task<IActionResult> GetAll(int? typeUserId, string? name, string? userName, bool? estado)
        {
            var users = await _user.GetAll(typeUserId, name, userName, estado);
            return Ok(users);
        }
        [HttpGet("{userId}")]
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int userId)
        {
            try
            {
                var users = await _user.GetById(userId);
                return Ok(users);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<UserResultDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] UserInsertDto insertDto)
        {
            try
            {
                var user = await _user.Insert(insertDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult<UserResultDto>> UpdateUser([FromBody] UserUpdateDto updateDto)
        {
            try
            {
                var user = await _user.Update(updateDto);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteUser([FromBody] UserDeleteDto deleteDto)
        {
            try
            {
                var result = await _user.Delete(deleteDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAssignedModules")]
        public async Task<ActionResult<UserModulesResultDto>> GetAssignedModules(string userName)
        {
            try
            {
                var result = await _user.GetAssignedModules(userName);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllModules")]
        public async Task<ActionResult<List<UserResultModuleDto>>> GetAllModules()
        {
            try
            {
                var result = await _user.GetAllModules();
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
