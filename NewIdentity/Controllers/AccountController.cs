using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using NewIdentity.Data;
using NewIdentity.Models;
using NewIdentity.ViewModels;
using System.Text;


namespace NewIdentity.Controllers
{

    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        // private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;
        //  private readonly IViewRenderService _viewRenderService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, SignInManager<ApplicationUser> signInManager)
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




        // just test 

        [HttpGet]
        public async Task<IActionResult> EditCountry()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var model = new EditCountryVM
            {
                SelectedCountryId = user.CountryId,
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
        public async Task<IActionResult> EditCountry(EditCountryVM model)
        {

            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.CountryId =  model.SelectedCountryId;
            await _userManager.UpdateAsync(user);           



            return await EditCountry();



            






            // HANDLING EDIT COUNTRY    
            //public async Task<IActionResult> EditCountry()
            //{
            //    var user = await _userManager.GetUserAsync(User);
            //    if (user == null) return RedirectToAction("Login");
            //    var model = new EditCountryVM
            //    {
            //         SelectedCountryId = user.CountryId,

            //        Countries = _dbContext.Countries
            //          .Select(c => new SelectListItem
            //          {
            //              Value = c.Id.ToString(),
            //              Text = c.Name,
            //              Selected = c.Id == user.CountryId
            //          }).ToList()

            //    };
            //    return View(model);
            //}
            //[HttpPost]
            //public async Task<IActionResult> EditCountry(EditCountryVM model)
            //{
            //    if (!ModelState.IsValid) return View(model);
            //    var user = await _userManager.GetUserAsync(User);
            //    if (user == null) return RedirectToAction("Login");
            //    user.CountryId = model.SelectedCountryId;
            //    await _userManager.UpdateAsync(user);
            //    await _dbContext.SaveChangesAsync();

            //    return RedirectToAction("Index", "Home");
            //}

        }

    }
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






