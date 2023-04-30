using Authentication.Models.DTOs.Requests;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Models.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<IdentityResult> RegisterAsync(UserRegistration request);
    }
}