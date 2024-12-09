using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewIdentity.Data;
using NewIdentity.Models;
using NewIdentity.ViewModels;
using System.CodeDom;
using NewIdentity.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace NewIdentity.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ManageCountryController : CustomBaseController
    {
        private readonly ApplicationDbContext _dbContext;
        public ManageCountryController(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var countries = _dbContext.Countries
                .Select(c => new CountryVM { Id = c.Id, Name = c.Name, }).ToList();
            return View(countries);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // create a new country; post
        [HttpPost]
        
        public async Task<IActionResult> Create(CountryVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var country = new Country { Name = model.Name };
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Edit Country get 
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var country = _dbContext.Countries.Find(id);
            if (country == null) return NotFound();

            var model = new CountryVM { Id = country.Id, Name = country.Name };
            return View(model);
        }
        // edit country post
        [HttpPost]
        public async Task <IActionResult> Edit(int id, CountryVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var country = await _dbContext.Countries.FindAsync(id);
            if (country == null) return NotFound();

            country.Name = model.Name;
           await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        // delete country get
        public IActionResult Delete(int id)
        {
            var country = _dbContext.Countries.Find(id);
            if (country == null) return NotFound();

            var model = new CountryVM {Id = country.Id, Name = country.Name };
            return View(model);
        }
        // delete country post 
        public async Task<IActionResult> DeleteConFirmed(int id)
        {
            var country = await _dbContext.Countries.FindAsync(id);
            if (country == null) return NotFound();

            bool hasAssignedUsers = _dbContext.Users.Any(u => u.CountryId == id);
            if (hasAssignedUsers)
            {
                TempData["ErrorMessage"] = $"{Resource.OOPS__Can_not_delete_this_country_because_there_are_users_assigned_to_it_}";
                return RedirectToAction("index");
            }

            _dbContext.Countries.Remove(country);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Country deleted successfully.";
            

            return RedirectToAction("Index");
        }
    }
}
