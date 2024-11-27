
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewIdentity.Data;
using NewIdentity.Models;
using NewIdentity.Tools;

var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddIdentity<Microsoft.AspNetCore.Identity.IdentityUser, Microsoft.AspNetCore.Identity.IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
    AddEntityFrameworkStores<ApplicationDbContext>().
    AddDefaultTokenProviders(); 
    

////the below is the previous code
//builder.Services.AddDefaultIdentity<ApplicationUser>().
//    AddEntityFrameworkStores<ApplicationDbContext>().
//    AddDefaultTokenProviders();



//builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IViewRenderService, ViewRenderService>();

//builder.Services.AddScoped<DataSeeder>();






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
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

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
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages()
       .WithStaticAssets();

    app.Run();


