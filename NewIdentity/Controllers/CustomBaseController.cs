using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace NewIdentity.Controllers
{
    public class CustomBaseController : Controller
    {
        public CustomBaseController(IHttpContextAccessor httpContextAccessor)
        {
            if ((httpContextAccessor?.HttpContext?.Request?.Cookies[CookieRequestCultureProvider.DefaultCookieName] ?? "en") == "en")
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            else
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa");
        }
    }
}
