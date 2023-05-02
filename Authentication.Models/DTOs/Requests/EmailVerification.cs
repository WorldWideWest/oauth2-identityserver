using System.ComponentModel.DataAnnotations;

namespace Authentication.Models.DTOs.Requests
{
    public class EmailVerification
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}