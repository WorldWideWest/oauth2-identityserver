using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Models.DTOs.Responses
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        private readonly List<IdentityError> _errors = new List<IdentityError>();
        
        [JsonIgnore]
        public bool Succeeded { get; protected set; } = true;
        
        public IEnumerable<IdentityError> Errors => _errors;

        public static TokenResult Success(string accessToken, string refreshToken, int expiresIn) =>
            new TokenResult() {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresIn,
            };

        public static TokenResult Failed(params IdentityError[] errors)
        {
            var result = new TokenResult { Succeeded = false };

            if (errors != null)
                result._errors.AddRange(errors);

            return result;
        }
    }
}