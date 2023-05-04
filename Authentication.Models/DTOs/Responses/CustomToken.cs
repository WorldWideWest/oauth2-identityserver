using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Models.DTOs.Responses
{
    public class CustomToken
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpiresIn { get; set; }

        private static readonly CustomToken _success = new CustomToken { Succeeded = true };
        
        private readonly List<IdentityError> _errors = new List<IdentityError>();
        
        [JsonIgnore]
        public bool Succeeded { get; protected set; }
        
        public IEnumerable<IdentityError> Errors => _errors;

        public static CustomToken Success => _success;

        public static CustomToken Failed(params IdentityError[] errors)
        {
            var result = new CustomToken { Succeeded = false };

            if (errors != null)
                result._errors.AddRange(errors);

            return result;
        }
    }
}