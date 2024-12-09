using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewIdentity.Data;
using NewIdentity.Models;
using System.CodeDom;

namespace NewIdentity.Controllers
{
    public class CountryController : CustomBaseController
    {
        private readonly ApplicationDbContext _dbContext;

        public CountryController(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> List()
        {
            var countries = await _dbContext.Countries.ToListAsync();
            return View("/Views/Country/List.cshtml", countries);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("/Views/Country/Create.cshtml", new Country());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Country country)
        {
            try
            {
                _dbContext.Countries.Add(country);
                await _dbContext.SaveChangesAsync();
                return Redirect("/Country/List");

            }
            catch (Exception)
            {
                return Redirect("/Country/List");
            }

        }
    }
}