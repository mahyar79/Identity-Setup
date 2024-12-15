using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization.Routing;

namespace NewIdentity.Cultures
{
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        private readonly IList<string> _supportedCultures;

        public RouteDataRequestCultureProvider(IList<string> supportedCultures)
        {
            _supportedCultures = supportedCultures;
        }

        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var culture = httpContext.Request.Path.Value?.Split('/')[1];
            var supportedCultures = new[] { "en", "fa" };

            if (!string.IsNullOrEmpty(culture) && supportedCultures.Contains(culture))
            {
                return Task.FromResult(new ProviderCultureResult(culture));
            }

            // Fallback to cookie if no culture in the route
            var cookieCulture = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            if (!string.IsNullOrEmpty(cookieCulture))
            {
                return Task.FromResult(new ProviderCultureResult(cookieCulture));
            }

            return Task.FromResult<ProviderCultureResult?>(null);
        }
    }
}
