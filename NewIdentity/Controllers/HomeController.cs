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



        // set language action 
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                // Update the culture cookie
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    culture
                );
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = $"/{culture}/Home/Index";
            }
            else
            {
               
                var segments = returnUrl.Split('/');
                if (segments.Length > 1 && segments[1].Length == 2)
                {
                    segments[1] = culture;
                    returnUrl = string.Join('/', segments);
                }
                else
                {
                    returnUrl = $"/{culture}{returnUrl}"; 
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
