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
      
           var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

           if (userIdClaim == null)
               return RedirectToAction("Index", "Login");

           if (int.TryParse(userIdClaim.Value, out int userId))
           {
               var user = _context.Users.SingleOrDefault(u => u.Id == userId);
               if (user != null)
               {
                   // Kullanıcı bilgilerini View'e gönder
                   var model = new Talep
                   {
                       Ad = user.Ad,
                       Soyad = user.Soyad,
                       Email = user.Email,
                   };

                   return View(model);
               }
           }

           return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public async Task<IActionResult> Create(Talep model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
                return View(model);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                //ModelState.AddModelError("", "Giriş yapmanız gerekiyor.");
                Console.WriteLine("Kullanıcı oturum açmamış.");
                return RedirectToAction("Index", "Login");
            }

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                try
                {
                    var talep = new Talep
                    {
                        UserId = userId,
                        Ad = model.Ad,
                        Soyad = model.Soyad,
                        Email = model.Email,
                        SicilNo = model.SicilNo,
                        Ariza = model.Ariza,
                        Bolum = model.Bolum,
                        Birim = model.Birim,
                        TelNo = model.TelNo,
                        Ilgilenen = model.Ilgilenen,
                        Durum = model.Durum
                    };

                    _context.Taleps.Add(talep);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Talebiniz başarıyla alınmıştır.";
                    return RedirectToAction("MyApplications");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Veritabanı Hatası: {ex.Message}");
                    ModelState.AddModelError("", "Talebiniz kaydedilemedi. Daha sonra tekrar deneyiniz.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı kimliği.");
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
