using Authentication.Models.DTOs.Requests;
using Authentication.Models.Entities.Identity;
using Authentication.Models.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;

namespace Authentication.Service
{
    public class IdentityService : IIdentityService
    {
        private readonly ILogger<IdentityService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEmailService _emailService;

        public IdentityService(
            ILogger<IdentityService> logger,
            UserManager<User> userManager,
            IPasswordHasher<User> passwordHasher,
            IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
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
                    UserName = request.Email,
                };

                newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

                var result = await _userManager.CreateAsync(newUser).ConfigureAwait(false);
                if(!result.Succeeded)
                    return result;
                
                User user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

                await _emailService.SendAsync(user.Email, "Confirm Email", $"Email:{user.Email}, Token:{token}").ConfigureAwait(false);

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(RegisterAsync));
                throw ex;
            }
        }
    }
}