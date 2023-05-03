using Authentication.Models.DTOs.Requests;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Models.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<IdentityResult> RegisterAsync(UserRegistration request);
        Task<IdentityResult> VerifyEmailAsync(EmailVerification request);
        Task<IdentityResult> ResetPasswordAsync(PasswordReset request);
        Task<IdentityResult> VerifyPasswordAsync(VerifyPassword request);
        Task<IdentityResult> ChangePasswordAsync(ChangePassword request);
        Task<IdentityResult> DeleteUserAsync(DeleteUser request);
    }
}