using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ariza.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly Context _context;
        public KullaniciController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var talepler = _context.Taleps.ToList(); // Burada `Talepler` sizin modeliniz.

            // Veriyi View'e gönder
            return View(talepler);
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
