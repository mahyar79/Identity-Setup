using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NewIdentity.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(250)]
        //[Remote("IsAnyUserName","Account",HttpMethod = "Post",AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Remote("IsAnyEmail", "Account", HttpMethod = "Post", AdditionalFields = "__RequestVerificationToken")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }

        public int? SelectedCountryId { get; set; }

        [Required]
        public string Role { get; set; }

        public List<SelectListItem> Countries { get; set; }
    }
}
