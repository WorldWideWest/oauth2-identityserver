using Authentication.Models.DTOs.Requests;
using Authentication.Models.DTOs.Responses;

namespace Authentication.Models.Interfaces.Services
{
    public interface ITokenService
    {
        Task<CustomToken> TokenAsync(Token request);
    }
}