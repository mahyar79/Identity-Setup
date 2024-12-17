using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NewIdentity.Models;
using System.Diagnostics;

namespace NewIdentity.Controllers
{
    public class HomeController : CustomBaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }






        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                culture = culture switch
                {
                    "en" => "en-US",
                    "fa" => "fa-IR",
                    _ => culture
                };

                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = $"/{culture}/Home/Index";
            }
            else
            {
                var segments = returnUrl.TrimStart('/').Split('/');

                if (segments.Length > 0 && (segments[0].Length == 2 || segments[0].Length == 5))
                {
                    segments[0] = culture;
                    returnUrl = "/" + string.Join('/', segments);
                }
                else
                {
                    returnUrl = $"/{culture}{(returnUrl.StartsWith("/") ? returnUrl : "/" + returnUrl)}";
                }
            }

            return LocalRedirect(returnUrl);
        }














        [Authorize(Roles = "Admin")]
        public class AdminController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }
        }

        [Authorize(Roles = "User")]
        public class UserController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }
        }



    }
}
