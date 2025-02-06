using Ariza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ariza.Helpers;

namespace Ariza.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(Context context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            _logger.LogInformation("Admin paneli görüntülendi.");
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            try
            {
                _logger.LogInformation("Kullanıcı listesi getiriliyor...");
                var users = await _context.GetAllUsersAsync();
                _logger.LogInformation("Kullanıcı listesi başarıyla getirildi. Toplam {UserCount} kullanıcı bulundu.", users.Count());

                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı listesi getirilirken bir hata oluştu.");
                ViewData["ErrorMessage"] = "Kullanıcı listesi getirilirken bir hata oluştu.";
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser()
        {
            ModelState.Clear();
            return View(new User());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User model)
        {
            if (string.IsNullOrEmpty(model.Ad) || string.IsNullOrEmpty(model.Soyad) || string.IsNullOrEmpty(model.Email))
            {
                _logger.LogWarning("Kullanıcı ekleme başarısız: Eksik alanlar mevcut.");
                ModelState.AddModelError(string.Empty, "Tüm alanlar doldurulmalıdır.");
                return View(model);
            }

            try
            {
                var newUser = new User
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Email = model.Email,
                    Sifre = model.Sifre
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Yeni kullanıcı eklendi: {UserEmail}", newUser.Email);

                var userRole = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = 2 //default olarak user rolü verildi
                };

                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Yeni kullanıcıya rol atandı: {UserId} - RoleId: {RoleId}", newUser.Id, userRole.RoleId);

                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı eklenirken bir hata oluştu: {UserEmail}", model.Email);
                ViewData["ErrorMessage"] = "Kullanıcı eklenirken bir hata oluştu.";
                return View("Error");
            }
        }

        public async Task<IActionResult> ApplicationList(string searchString, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Başvuru listesi getiriliyor...");

                var taleps = _context.Taleps.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    taleps = taleps.Where(t => t.Ad.Contains(searchString) || t.Soyad.Contains(searchString));
                    _logger.LogInformation("Başvurular {SearchString} ile filtrelendi.", searchString);
                }

                taleps = taleps.OrderByDescending(u => u.CreatedDate);
                var paginatedList = await PaginatedList<Talep>.CreateAsync(taleps.AsNoTracking(), pageIndex, pageSize);

                _logger.LogInformation("Başvurular başarıyla getirildi. Toplam {TalepCount} başvuru bulundu.", paginatedList.Count);

                return View(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Başvuru listesi getirilirken bir hata oluştu.");
                ViewData["ErrorMessage"] = "Başvuru listesi getirilirken bir hata oluştu.";
                return View("Error");
            }
        }

        public async Task<IActionResult> Rapor(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                {
                    _logger.LogWarning("Geçersiz tarih aralığı: Başlangıç tarihi ({StartDate}) bitiş tarihinden ({EndDate}) büyük olamaz.", startDate, endDate);
                    ModelState.AddModelError("", "Başlangıç tarihi bitiş tarihinden büyük olamaz.");
                    return View(new PaginatedList<Talep>(new List<Talep>(), 0, pageIndex, pageSize));
                }

                _logger.LogInformation("Rapor oluşturuluyor... Tarih aralığı: {StartDate} - {EndDate}", startDate, endDate);

                var taleps = _context.Taleps.AsQueryable();

                if (startDate.HasValue && endDate.HasValue)
                {
                    taleps = taleps.Where(t => t.CreatedDate >= startDate && t.CreatedDate <= endDate);
                }

                taleps = taleps.OrderByDescending(t => t.CreatedDate);
                var paginatedList = await PaginatedList<Talep>.CreateAsync(taleps.AsNoTracking(), pageIndex, pageSize);

                _logger.LogInformation("Rapor başarıyla oluşturuldu. Toplam {TalepCount} başvuru bulundu.", paginatedList.Count);

                ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
                ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");

                return View(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rapor oluşturulurken bir hata oluştu.");
                ViewData["ErrorMessage"] = "Rapor oluşturulurken bir hata oluştu.";
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ChangeRole(int userId)
        {
            try
            {
                // Kullanıcıyı bul
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    _logger.LogError("ChangeRole: Kullanıcı bulunamadı. Kullanıcı ID: {UserId}", userId);
                    return NotFound("Kullanıcı bulunamadı.");
                }

                // Kullanıcının mevcut rolünü bul
                var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
                if (userRole == null)
                {
                    _logger.LogWarning("ChangeRole: Kullanıcı rolü atanmış değil. Kullanıcı ID: {UserId}", userId);
                }

                // Tüm rolleri getir
                var allRoles = await _context.Roles.ToListAsync();

                var model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    Ad = user.Ad,
                    Soyad = user.Soyad,
                    Email = user.Email,
                    CurrentRoleId = userRole?.RoleId ?? 0, // Kullanıcının rolü yoksa 0 döndür
                    AvailableRoles = allRoles
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeRole: Kullanıcı bulunurken bir hata oluştu.");
                return View("Error", new { message = "Kullanıcı rol bilgisi yüklenirken hata oluştu." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangeRole(ChangeRoleViewModel model)
        {
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == model.UserId);

            if (userRole != null)
            {
                userRole.RoleId = model.SelectedRoleId;
            }
            else
            {
                var newUserRole = new UserRole
                {
                    UserId = model.UserId,
                    RoleId = model.SelectedRoleId
                };
                _context.UserRoles.Add(newUserRole);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kullanıcı rolü başarıyla güncellendi!";

            return RedirectToAction("UserList");
        }
    }
}
