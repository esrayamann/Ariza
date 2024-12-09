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
        public async Task<IActionResult> ApplicationList( string searchString)
        {           
            var taleps = _context.Taleps.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                taleps = taleps.Where(t => t.Ad.Contains(searchString) || t.Soyad.Contains(searchString));
            }

            taleps = taleps.OrderByDescending(u => u.CreatedDate);

            var sortedTaleps = await taleps.AsNoTracking().ToListAsync();
            return View(sortedTaleps);
        }
        public async Task<IActionResult> Rapor(DateTime? startDate, DateTime? endDate)
        {
            var taleps = _context.Taleps.AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                taleps = taleps.Where(t => t.CreatedDate >= startDate && t.CreatedDate <= endDate);
            }

            taleps = taleps.OrderByDescending(t => t.CreatedDate);

            var filteredTaleps = await taleps.AsNoTracking().ToListAsync();

            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
            return View(filteredTaleps);
        }
    }
}


