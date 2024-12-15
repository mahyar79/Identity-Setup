using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

public class CustomBaseController : Controller
{
    public CustomBaseController(IHttpContextAccessor httpContextAccessor)
    {
        var culture = httpContextAccessor.HttpContext?.GetRouteValue("culture")?.ToString();

        if (string.IsNullOrEmpty(culture))
        {
            // Fallback to cookie value if route value is not present
            culture = httpContextAccessor.HttpContext?.Request?.Cookies[CookieRequestCultureProvider.DefaultCookieName]?.Split('|')[0] ?? "en";
        }

        if (culture == "en" || culture == "fa")
        {
            var cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
