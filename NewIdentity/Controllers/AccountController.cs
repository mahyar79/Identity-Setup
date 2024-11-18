using System.Security.Claims;
using System.Text;
using NewIdentity.Models;
using NewIdentity.Tools;
using NewIdentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace NewIdentity.Controllers
{

    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IViewRenderService _viewRenderService;

        public AccountController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public IActionResult Register()
        {
            ViewBag.IsSent = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userManager.CreateAsync(new IdentityUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone
            }, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            var toekn = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            toekn = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(toekn));
            string? callBackurl = Url.ActionLink("ConfirmEmail", "Account", new { UserId = user.Id, token = toekn
                , Request.Scheme });

            string body = await _viewRenderService.RenderToStringAsync("_RegisterEmail", callBackurl);
            await _emailSender.SendEmailAsync(new EmailModel(user.Email, "Confirmation", body));

            ViewBag.IsSent = true;
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return BadRequest();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            ViewBag.IsConfirmed = result.Succeeded  ? true : false;
            return View();
        }
    }
}
