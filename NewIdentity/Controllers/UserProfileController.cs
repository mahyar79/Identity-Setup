using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewIdentity.Data;
using NewIdentity.Models;
using NewIdentity.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace NewIdentity.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
       private readonly UserManager<ApplicationUser> _userManager;
       private readonly ApplicationDbContext _dbContext;

        public UserProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
                _dbContext = dbContext;
                _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) RedirectToAction("Login", "Account");

            var model = new EditProfileVM
            {
                FirstName = user.FirstName,
                FamilyName = user.FamilyName,
                SelectedCountryId = user.CountryId ?? 0,
                Countries = _dbContext.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = _dbContext.Countries
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();        
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null) RedirectToAction("Login", "Action");

            user.FirstName = model.FirstName;
            user.FamilyName = model.FamilyName;
            user.CountryId = model.SelectedCountryId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                model.Countries = _dbContext.Countries
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();
                return View(model);
            }
            return RedirectToAction("Index", "Home");
            

        }
    }
}
