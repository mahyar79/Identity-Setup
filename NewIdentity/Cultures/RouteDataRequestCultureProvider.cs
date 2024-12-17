//using Microsoft.AspNetCore.Localization;
//using Microsoft.AspNetCore.Http;
//using System.Globalization;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//namespace NewIdentity.Cultures
//{
//    public class RouteDataRequestCultureProvider : RequestCultureProvider
//    {
//        private readonly IList<string> _supportedCultures;

//        public RouteDataRequestCultureProvider(IList<string> supportedCultures)
//        {
//            _supportedCultures = supportedCultures;
//        }

//        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
//        {
//            var culture = httpContext.Request.Path.Value?.Split('/')[1];

//            if (!string.IsNullOrEmpty(culture) && _supportedCultures.Contains(culture))
//            {
//                // If culture is in the route, use it
//                return Task.FromResult(new ProviderCultureResult(culture switch
//                {
//                    "en" => "en-US",
//                    "fa" => "fa-IR",
//                    _ => culture
//                }));
//            }

//            // Fallback to the cookie if no culture is in the route
//            var cookieCulture = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
//            if (!string.IsNullOrEmpty(cookieCulture))
//            {
//                return Task.FromResult(new ProviderCultureResult(cookieCulture));
//            }

//            return Task.FromResult<ProviderCultureResult?>(null);
//        }

//    }
//}




using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

            if (!string.IsNullOrEmpty(culture) && _supportedCultures.Contains(culture))
            {
                var fullCulture = culture switch
                {
                    "en" => "en-US",
                    "fa" => "fa-IR",
                    _ => culture
                };
                return Task.FromResult(new ProviderCultureResult(fullCulture));
            }

            var cookieCulture = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            if (!string.IsNullOrEmpty(cookieCulture))
            {
                return Task.FromResult(new ProviderCultureResult(cookieCulture));
            }

            return Task.FromResult<ProviderCultureResult?>(null);
        }
    }
}
