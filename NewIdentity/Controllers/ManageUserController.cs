using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewIdentity.Models;
using NewIdentity.ViewModels;

namespace NewIdentity.Controllers
{
    public class ManageUserController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageUserController(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var totalUsers = await _userManager.Users.CountAsync(); 

            var users = await _userManager.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
           .Select(user => new UserVM
           {
               Id = user.Id,
               UserName = user.UserName,
               FirstName = user.FirstName,
               LastName = user.FamilyName,
               Email = user.Email
           })
           .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

            ViewBag.currentPage = page;
            ViewBag.totalPages = totalPages;

            return View(users);
        }
    }
}
