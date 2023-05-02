using Authentication.Models.DTOs.Requests;
using Authentication.Models.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly IIdentityService _identityService;

        public IdentityController(
            ILogger<IdentityController> logger,
            IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }
        
        [HttpPost("register")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdentityResult>> RegisterAsync([FromBody] UserRegistration request)
        {
            try
            {
                var result = await _identityService.RegisterAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(RegisterAsync));
                throw ex;
            }
        }

        [HttpPost("verify")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdentityResult>> VerifyEmailAsync([FromBody] EmailVerification request)
        {
            try
            {
                var result = await _identityService.VerifyEmailAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(VerifyEmailAsync));
                throw ex;
            }
        }

        [HttpPost("password/reset")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdentityResult>> ResetPasswordAsync([FromBody] PasswordReset request)
        {
            try
            {
                var result = await _identityService.ResetPasswordAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(ResetPasswordAsync));
                throw ex;
            }
        }
        
        [HttpPost("password/verify")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdentityResult>> VerifyPasswordAsync([FromBody] VerifyPassword request)
        {
            try
            {
                var result = await _identityService.VerifyPasswordAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(VerifyPasswordAsync));
                throw ex;
            }
        }
    }
}