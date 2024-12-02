using Ariza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ariza.Controllers
{
    public class TalepController : Controller
    {
        private readonly Context _context;

        public TalepController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BasvuruViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    ModelState.AddModelError("", "Giriş yapmanız gerekiyor.");
                    return RedirectToAction("Index", "Login");
                }

                if (int.TryParse(userIdClaim.Value, out int userId))
                {
                    var talep = new Talep
                    {
                        UserId = userId,
                        Ariza = model.Ariza,
                        Bolum = model.Bolum
                    };

                    // Context ile başvuru ekleme
                    _context.Taleps.Add(talep);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Success");
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz kullanıcı kimliği.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return RedirectToAction("Index", "Login");

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                // Context ile başvuruları getirme
                var basvurular = await _context.Taleps
                    .Where(b => b.UserId == userId)
                    .ToListAsync();

                return View(basvurular);
            }

            return RedirectToAction("Index", "Login");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
