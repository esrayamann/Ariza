using Ariza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ariza.Controllers
{
    [Authorize]
    public class TalepController : Controller
    {
        private readonly Context _context;

        public TalepController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(MyApplications));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BasvuruViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim?.Value, out var userId))
            {
                return Challenge();
            }

            var requester = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(user => user.Id == userId);
            if (requester is null)
            {
                return Challenge();
            }

            var talep = new Talep
            {
                UserId = userId,
                Ad = requester.Ad,
                Soyad = requester.Soyad,
                Email = requester.Email,
                Ilgilenen = "Atanmadı",
                Ariza = model.Ariza.Trim(),
                Birim = model.Birim.Trim(),
                Bolum = model.Bolum.Trim(),
                Durum = TalepDurumlari.Yeni,
                CreatedDate = DateTime.UtcNow
            };

            _context.Taleps.Add(talep);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Success));
        }

        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdClaim?.Value, out int userId))
            {
                var basvurular = await _context.Taleps
                    .AsNoTracking()
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.CreatedDate)
                    .ToListAsync();

                return View(basvurular);
            }

            return Challenge();
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
