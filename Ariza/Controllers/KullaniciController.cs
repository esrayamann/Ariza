using Ariza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ariza.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<KullaniciController> _logger;

        public KullaniciController(Context context, ILogger<KullaniciController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var taleps = _context.Taleps.OrderByDescending(t => t.CreatedDate);
                var paginatedList = await PaginatedList<Talep>.CreateAsync(taleps.AsNoTracking(), pageIndex, pageSize);
                return View(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the applications. PageIndex: {PageIndex}, PageSize: {PageSize}", pageIndex, pageSize);
                return View("Error");
            }
        }

        public async Task<IActionResult> ApplicationList(string searchString, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var taleps = _context.Taleps.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    taleps = taleps.Where(t => t.Ad.ToLower().Contains(searchString) || t.Soyad.ToLower().Contains(searchString));
                }

                taleps = taleps.OrderByDescending(u => u.CreatedDate);

                var paginatedList = await PaginatedList<Talep>.CreateAsync(taleps.AsNoTracking(), pageIndex, pageSize);
                return View(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching applications.");
                return View("Error");
            }
        }

        public async Task<IActionResult> Rapor(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                {
                    ModelState.AddModelError("", "Başlangıç tarihi bitiş tarihinden büyük olamaz.");
                    return View(new PaginatedList<Talep>(new List<Talep>(), 0, pageIndex, pageSize));
                }

                var taleps = _context.Taleps.AsQueryable();

                if (startDate.HasValue && endDate.HasValue)
                {
                    taleps = taleps.Where(t => t.CreatedDate >= startDate && t.CreatedDate <= endDate);
                }

                taleps = taleps.OrderByDescending(t => t.CreatedDate);

                var paginatedList = await PaginatedList<Talep>.CreateAsync(taleps.AsNoTracking(), pageIndex, pageSize);

                ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

                return View(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the report.");
                return View("Error");
            }
        }
    }
}
