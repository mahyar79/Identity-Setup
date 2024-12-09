
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewIdentity.Data;
using NewIdentity.Models;
using NewIdentity.Tools;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Localization.Routing;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
    AddEntityFrameworkStores<ApplicationDbContext>().
    AddDefaultTokenProviders();




//builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IViewRenderService, ViewRenderService>();

//builder.Services.AddScoped<DataSeeder>();

// add localization servies
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");




// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}





// localization middleware 
var supportedCultures = new[] { "en", "fa" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

//add the custom RouteDataRequestCultureProvider 
localizationOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());


app.UseRequestLocalization(localizationOptions);

// calling seeding logic 
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var dataSeeder = new DataSeeder(roleManager);
    await dataSeeder.SeedRolesAsync();
}




app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{culture = en}/{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
