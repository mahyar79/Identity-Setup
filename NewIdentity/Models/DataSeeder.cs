using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewIdentity.Models;
using System.Threading.Tasks;

public class DataSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public DataSeeder(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
    }
}