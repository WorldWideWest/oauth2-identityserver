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
        [ProducesResponseType(typeof(IdentityResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<IdentityError>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IdentityResult>> RegisterAsync(UserRegistration request)
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

    }
}