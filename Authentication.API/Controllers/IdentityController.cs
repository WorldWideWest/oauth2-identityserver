using Authentication.Models.Constants.Identity;
using Authentication.Models.DTOs.Requests;
using Authentication.Models.DTOs.Responses;
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
        private readonly ITokenService _tokenService;

        public IdentityController(
            ILogger<IdentityController> logger,
            IIdentityService identityService,
            ITokenService tokenService)
        {
            _logger = logger;
            _identityService = identityService;
            _tokenService = tokenService;
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

        [HttpPost("password/change")]
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdentityResult>> ChangePasswordAsync([FromBody] ChangePassword request)
        {
            try
            {
                var result = await _identityService.ChangePasswordAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(ChangePasswordAsync));
                throw ex;
            }
        }

        [HttpDelete("deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdentityResult>> DeleteUserAsync([FromBody] DeleteUser request)
        {
            try
            {
                var result = await _identityService.DeleteUserAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(DeleteUserAsync));
                throw ex;
            }
        }
        
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenResult>> TokenAsync([FromBody] Token request)
        {
            try
            {
                var result = await _tokenService.TokenAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(TokenAsync));
                throw ex;
            }
        }

        [HttpPost("token/refresh")]
        [ProducesResponseType(typeof(TokenResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenResult>> RefreshTokenAsync([FromBody] Token request)
        {
            try
            {
                request.GrantType = GrantType.RefreshToken;

                var result = await _tokenService.TokenAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(TokenAsync));
                throw ex;
            }
        }

        [HttpPost("token/revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenResult>> RevokeTokenAsync([FromBody] Token request)
        {
            try
            {
                var result = await _tokenService.RevokeAsync(request).ConfigureAwait(false);
                
                if(!result.Succeeded)
                    return BadRequest(result.Errors);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(RevokeTokenAsync));
                throw ex;
            }
        }
    }
}