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
using Microsoft.AspNetCore.Mvc.Rendering;
using NewIdentity.Data;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace NewIdentity.Controllers
{

    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        // private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;
        //  private readonly IViewRenderService _viewRenderService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            // _emailSender = emailSender;
            _dbContext = dbContext;
            _signInManager = signInManager;
        }


        // Get: Login 
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVM());
        }


        // now post
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account is locked out.");
                return View(model);
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);


        }



        // get for logOut is for test only. delete if needed 

        //[HttpGet]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction("Index", "Home");
        //}




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout(string? returnUrl = null)
        //{
        //    await _signInManager.SignOutAsync();
        //    returnUrl ??= Url.Action("Index", "Home");
        //    return LocalRedirect(returnUrl);
        //}






        // post logout 

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public IActionResult Register()
        {

            var countries = _dbContext.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();


            var model = new RegisterVM
            {
                Countries = countries
            };

            ViewBag.IsSent = false;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {

            //if (!ModelState.IsValid)
            //{
            //    model.Countries = _dbContext.Countries
            //        .Select(c => new SelectListItem
            //        {
            //            Value = c.Id.ToString(),
            //            Text = c.Name
            //        }).ToList();
            //    return View(model);
            //}


            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                CountryId = model.SelectedCountryId
            };

            var result = await _userManager.CreateAsync(user, model.Password);


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

            // these two lines is for checking if it works for redirecting to home
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");





            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));


            string? callBackUrl = Url.ActionLink("ConfirmEmail", "Account", new
            {
                UserId = user.Id,
                token = token,
                Request.Scheme
            });


            // string body = await _viewRenderService.RenderToStringAsync("_RegisterEmail", callBackUrl);
            //  await _emailSender.SendEmailAsync(new EmailModel(user.Email, "Confirmation", body));





            ViewBag.IsSent = false;
            return View();
        }

        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    if (userId == null || token == null) return BadRequest();
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null) return NotFound();

        //    token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        //    var result = await _userManager.ConfirmEmailAsync(user, token);
        //    ViewBag.IsConfirmed = result.Succeeded ? true : false;
        //    return View();
        //}



    }
}
