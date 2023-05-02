using System.Security.Claims;
using Authentication.Models.DTOs.Requests;
using Authentication.Models.Entities.Identity;
using Authentication.Models.Interfaces.Services;
using IdentityModel;
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

        public async Task<IdentityResult> VerifyEmailAsync(EmailVerification request)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                {
                    IdentityError error = new()
                    {
                        Code = "404",
                        Description = "User Not Found"
                    };

                    return IdentityResult.Failed(error);
                }

                var result = await _userManager.ConfirmEmailAsync(user, request.Token).ConfigureAwait(false);

                if(!result.Succeeded)
                    return IdentityResult.Failed(result.Errors.ToArray());

                var claimsResult = await _userManager.AddClaimsAsync(user, new Claim[]{
                    new Claim(JwtClaimTypes.Email, user.Email),
                }).ConfigureAwait(false);

                if (!claimsResult.Succeeded)
                    throw new Exception(claimsResult.Errors.ToString());

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(VerifyEmailAsync));
                throw ex;
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(PasswordReset request)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                {
                    IdentityError error = new()
                    {
                        Code = "404",
                        Description = "User Not Found"
                    };

                    return IdentityResult.Failed(error);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _emailService.SendAsync(user.Email, "Password Reset", $"{user.Email}, Token: {token}");

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(ResetPasswordAsync));
                throw ex;
            }
        }

        public async Task<IdentityResult> VerifyPasswordAsync(VerifyPassword request)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                {   
                    IdentityError error = new()
                    {
                        Code = "404",
                        Description = "User Not Found"
                    }; 

                    return IdentityResult.Failed(error);
                }

                var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
 
                if(!result.Succeeded)
                    return IdentityResult.Failed(result.Errors.ToArray());

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(VerifyPasswordAsync));
                throw ex;
            }
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePassword request)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                {   
                    IdentityError error = new()
                    {
                        Code = "404",
                        Description = "User not found",
                    };

                    return IdentityResult.Failed(error);
                }

                var verifyOldPasswordResult = await _userManager.CheckPasswordAsync(user, request.OldPassword);
                if(!verifyOldPasswordResult)
                {
                    IdentityError error = new()
                    {
                        Code = "409",
                        Description = "Password Not Matching",
                    };

                    return IdentityResult.Failed(error); 
                }

                var newPasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
                user.PasswordHash = newPasswordHash;

                var result = await _userManager.UpdateAsync(user);
                if(!result.Succeeded)
                    return IdentityResult.Failed(result.Errors.ToArray());

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, nameof(ChangePasswordAsync));
                throw ex;
            }
        }
    }
}