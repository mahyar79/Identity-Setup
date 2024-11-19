using Microsoft.AspNetCore.Identity;

namespace NewIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
        // Include => join tables
    }
}