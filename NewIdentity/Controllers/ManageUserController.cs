using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewIdentity.Models;
using NewIdentity.ViewModels;

namespace NewIdentity.Controllers
{
    public class ManageUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageUserController(UserManager<ApplicationUser> userManager)
        {
                _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
           .Select(user => new UserVM
           {
               Id = user.Id,
               UserName = user.UserName,
               FirstName = user.FirstName,
               LastName = user.FamilyName,
               Email = user.Email
           })
           .ToListAsync();
            return View(users);
        }
    }
}
