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
                //try
                //{
                //    var talep = new Talep
                //    {
                //        UserId = userId,
                //        Ad = model.Ad,
                //        Soyad = model.Soyad,
                //        Email = model.Email,
                //        SicilNo = model.SicilNo,
                //        Ariza = model.Ariza,
                //        Bolum = model.Bolum,
                //        Birim = model.Birim,
                //        TelNo = model.TelNo,
                //        Ilgilenen = model.Ilgilenen,
                //        Durum = model.Durum
                //    };

                //    _context.Taleps.Add(talep);
                //    await _context.SaveChangesAsync();

                //    TempData["SuccessMessage"] = "Talebiniz başarıyla alınmıştır.";
                //    return RedirectToAction("MyApplications");
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"Veritabanı Hatası: {ex.Message}");
                //    ModelState.AddModelError("", "Talebiniz kaydedilemedi. Daha sonra tekrar deneyiniz.");
                //}
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
    }
}
