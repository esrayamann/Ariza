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
                ModelState.AddModelError(string.Empty, "Tüm alanlar doldurulmalıdır.");
                return View(model);
            }

            if (model == null)
            {
                return BadRequest("Kullanıcı bilgileri eksik.");
            }

            var newUser = new User
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                Email = model.Email,
                Sifre = model.Sifre
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            
            var userRole = new UserRole
            {
                UserId = newUser.Id,
                RoleId = 2 //default olarak user rolü verildi
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> ApplicationList(string searchString, int pageIndex = 1, int pageSize = 10)
        {
            var taleps = _context.Taleps.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                taleps = taleps.Where(t => t.Ad.Contains(searchString) || t.Soyad.Contains(searchString));
            }

            taleps = taleps.OrderByDescending(u => u.CreatedDate); //tarihe göre sıralama

            var paginatedList = await PaginatedList<Talep>.CreateAsync(taleps.AsNoTracking(), pageIndex, pageSize);
            return View(paginatedList);
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


