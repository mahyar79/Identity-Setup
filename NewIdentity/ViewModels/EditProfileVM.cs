using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewIdentity.ViewModels
{
    public class EditProfileVM
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [Required]
        [Display(Name = "country")]
        public int SelectedCountryId {  get; set; }

        public List<SelectListItem>? Countries { get; set; }
    }
}
