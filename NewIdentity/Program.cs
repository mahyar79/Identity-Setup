using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewIdentity.Data;
using NewIdentity.Models;
using NewIdentity.Tools;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using NewIdentity.Cultures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IViewRenderService, ViewRenderService>();

// Localization configuration
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "fa-IR" };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures.Select(culture => new CultureInfo(culture)).ToList();
    options.SupportedUICultures = supportedCultures.Select(culture => new CultureInfo(culture)).ToList();

    // Add the custom RouteDataRequestCultureProvider
    options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider(supportedCultures));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseRouting();


// Middleware to handle root requests and redirect to default culture
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.Trim('/');
    if (string.IsNullOrEmpty(path)) // If root request "/"
    {
        var defaultCulture = "en-US"; // Set your default culture
        context.Response.Redirect($"/{defaultCulture}/Home/Index", permanent: false);
        return;
    }
    await next();
});

// Apply localization middleware
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Middleware to redirect short culture codes (e.g., "/fa" to "/fa-IR")
app.Use(async (context, next) =>
{
    var culture = context.GetRouteValue("culture")?.ToString();

    if (culture == "fa" || culture == "en")
    {
        var fullCulture = culture == "fa" ? "fa-IR" : "en-US";
        var newPath = context.Request.Path.Value!.Replace($"/{culture}", $"/{fullCulture}");
        context.Response.Redirect(newPath + context.Request.QueryString, permanent: true);
        return;
    }

    await next();
});

// Seeding logic
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var dataSeeder = new DataSeeder(roleManager);
    await dataSeeder.SeedRolesAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{culture=en-US}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
