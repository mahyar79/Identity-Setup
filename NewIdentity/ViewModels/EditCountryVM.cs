using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewIdentity.ViewModels
{
    public class EditCountryVM
    {
        public int? SelectedCountryId { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}
