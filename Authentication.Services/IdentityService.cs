using Authentication.Models.DTOs.Requests;
using Authentication.Models.Entities.Identity;
using Authentication.Models.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Authentication.Service
{
    public class IdentityService : IIdentityService
    {
        private readonly ILogger<IdentityService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public IdentityService(ILogger<IdentityService> logger, UserManager<User> userManager, PasswordHasher<User> passwordHasher)
        {
            _logger = logger;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> RegisterAsync(UserRegistration request)
        {
            try
            {
                var exsistingUser = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);

                if(exsistingUser is not null)
                {
                    IdentityError errors = new()
                    {
                        Code = "409",
                        Description = "User Already Exists"
                    };
                    
                    return IdentityResult.Failed(errors);
                }

                User newUser = new() {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    UserName = request.Email
                };

                newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

                var result = await _userManager.CreateAsync(newUser).ConfigureAwait(false);
                if(!result.Succeeded)
                    return result;
                
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}