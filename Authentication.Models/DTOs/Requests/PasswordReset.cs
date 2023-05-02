using System.ComponentModel.DataAnnotations;

namespace Authentication.Models.DTOs.Requests
{
    public class PasswordReset
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class VerifyPassword : PasswordReset
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}