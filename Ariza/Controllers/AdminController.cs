using Ariza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ariza.Controllers
{
    public class AdminController : Controller
    {
        private readonly Context _context;
        public AdminController(Context context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            var users = await _context.GetAllUsersAsync();
           

            return View(users);

        }
        //public async Task<IActionResult> ApplicationList()
        //{
        //    var taleps = await _context.GetAllApplicationsAsync();
        //    return View(taleps);
        //}
        ////
        public async Task<IActionResult> ApplicationList(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SortByNameAsc"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SortByDateDesc"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["SortByDateAsc"] = sortOrder == "DateAsc" ? "date_desc" : "DateAsc";


            var taleps = _context.Taleps.AsQueryable();


            // Arama
            if (!String.IsNullOrEmpty(searchString))
            {
                taleps = taleps.Where(t => t.Ad.Contains(searchString) || t.Soyad.Contains(searchString));
            }

            // Sıralama 
            taleps = sortOrder switch
            {
                "name_desc" => taleps.OrderByDescending(t => t.Ad),
                "Date" => taleps.OrderBy(t => t.CreatedDate),
                "date_desc" => taleps.OrderByDescending(t => t.CreatedDate),
                "DateAsc" => taleps.OrderBy(t => t.CreatedDate),
                _ => taleps.OrderBy(t => t.Ad),
            };

            var sortedTaleps = await taleps.AsNoTracking().ToListAsync();
            return View(sortedTaleps);
        }
    }
}


