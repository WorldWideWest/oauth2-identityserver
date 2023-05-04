using Authentication.Models.DTOs.Requests;
using Authentication.Models.DTOs.Responses;

namespace Authentication.Models.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokenResult> TokenAsync(Token request);
        Task<TokenResult> RevokeAsync(Token request);
    }
}