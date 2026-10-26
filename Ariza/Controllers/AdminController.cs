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
        public async Task<IActionResult> Index()
        {
            var talepler = await _context.Taleps
                .AsNoTracking()
                .Include(talep => talep.User)
                .OrderByDescending(talep => talep.CreatedDate)
                .ToListAsync();
            return View(talepler);
        }
    }
}


