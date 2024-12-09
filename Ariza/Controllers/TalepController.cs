using Ariza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            model.UserId = Convert.ToInt32(userIdClaim.Value);
            var user = _context.Users.SingleOrDefault(u => u.Id == model.UserId);

            if(userIdClaim != null && user != null)
                ModelState.Remove("User");


            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                 return View(model);
            }


            if (userIdClaim == null)
                return RedirectToAction("Index", "Login");
            

            if (int.TryParse(userIdClaim.Value, out int userId))
            {

                model.UserId = userId;
                try
                {
                    _context.Taleps.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Talebiniz başarıyla alınmıştır.";
                    return RedirectToAction("MyApplications");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                    ModelState.AddModelError("", "Talebiniz kaydedilemedi. Lütfen tekrar deneyin.");
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

        //public IActionResult Guncelle(Talep talep)
        //{
        //    var user = _context.Taleps.Find(talep.Id);
        //    user.Ad =talep.Ad;
        //    _context.SaveChanges();
        //    return RedirectToAction("MyApplications","Talep");
        //}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var talep = _context.Taleps.SingleOrDefault(t => t.Id == id);

            if (talep == null)
                return NotFound();

            return View(talep);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Talep model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(model);
            }

            var guncelTalep = await _context.Taleps.FindAsync(model.Id);
            if (guncelTalep == null)
                return NotFound();

            // Güncellenebilir alanları atama
            guncelTalep.SicilNo = model.SicilNo;
            guncelTalep.Ad = model.Ad;
            guncelTalep.Soyad = model.Soyad;
            guncelTalep.TelNo = model.TelNo;
            guncelTalep.Email = model.Email;
            guncelTalep.Ariza = model.Ariza;
            guncelTalep.Birim = model.Birim;
            guncelTalep.Bolum = model.Bolum;

            try
            {
                _context.Taleps.Update(guncelTalep);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Başarıyla güncellendi.";
                return RedirectToAction("MyApplications");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                ModelState.AddModelError("", "Talep güncellenemedi. Lütfen tekrar deneyin.");
            }

            return View(model); 
        }

        //

        [HttpGet]
        public IActionResult GetTalepInfo(string sicilNo)
        {
            if (string.IsNullOrEmpty(sicilNo))
            {
                return Json(new { success = false, message = "Sicil numarası boş olamaz." });
            }

            // Taleps tablosundan sicil numarasına göre veri getir
            var talep = _context.Taleps.SingleOrDefault(t => t.SicilNo == sicilNo);

            if (talep == null)
            {
                return Json(new { success = false, message = "Bu sicil numarasıyla eşleşen kayıt bulunamadı." });
            }

            // Gerekli bilgileri döndür
            return Json(new
            {
                success = true,
                data = new
                {
                    talep.Ad,
                    talep.Soyad,
                    talep.Bolum,
                    talep.Birim,
                    talep.TelNo,
                    talep.Ariza // Eğer arıza açıklamasını da doldurmak istiyorsanız
                }
            });
        }

    }
}
