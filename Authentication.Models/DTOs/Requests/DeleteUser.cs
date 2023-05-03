using System.ComponentModel.DataAnnotations;

namespace Authentication.Models.DTOs.Requests
{
    public class DeleteUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}