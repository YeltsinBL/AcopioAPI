using AcopioAPIs.DTOs.Login;
using AcopioAPIs.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcopioAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorization _authorization;

        public AuthorizationController(IAuthorization authorization)
        {
            _authorization = authorization;
        }
        
        [HttpPost("VerifyPassword")]
        public async Task<IActionResult> VerifyRegisterPassword([FromBody] AuthorizationRequest authorizationRequest)
        {
            var result = await _authorization.VerifyRegisterPassword(authorizationRequest);
            return Ok(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> RegisterNewPassword([FromBody] RegisterPasswordRequest resetPasswordRequest)
        {
            try
            {
                var result = await _authorization.RegisterNewPassword(resetPasswordRequest);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("LogIn")]
        public async Task<ActionResult<AuthorizationResponse>> LogIn([FromBody] AuthorizationRequest authorizationRequest)
        {
            try
            {
                var result = await _authorization.TokenResponse(authorizationRequest);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<AuthorizationResponse>> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                var result = await _authorization.RefreshTokenResponse(refreshTokenRequest);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
