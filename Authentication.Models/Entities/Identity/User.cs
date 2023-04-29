using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Models.Entities.Identity
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName  { get; set; }

        public string Picture { get; set; }
        
        [ProtectedPersonalData]
        [MaxLength(100)]
        public string Address { get; set; }
    }
}