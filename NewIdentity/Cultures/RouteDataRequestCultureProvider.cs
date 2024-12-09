using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace NewIdentity.Cultures
{
    // below is the previous version. 

    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var culture = httpContext.Request.Path.Value?.Split('/')[1];

            var supportedCultures = new[] { "en", "fa" };

            if (!string.IsNullOrEmpty(culture) && supportedCultures.Contains(culture))
            {
                return Task.FromResult(new ProviderCultureResult(culture));
            }
            return Task.FromResult<ProviderCultureResult?>(null);

        }
    }
}