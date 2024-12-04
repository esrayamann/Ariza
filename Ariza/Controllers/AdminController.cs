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
            var taleps = _context.Taleps.AsQueryable();


            // Arama
            if (!String.IsNullOrEmpty(searchString))
            {
                taleps = taleps.Where(t => t.Ad.Contains(searchString) || t.Soyad.Contains(searchString));
            }

            var sortedTaleps = await taleps.AsNoTracking().ToListAsync();
            return View(sortedTaleps);
        }
    }
}


