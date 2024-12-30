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

            // Yeni kullanıcıyı ekliyoruz
            var newUser = new User
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                Email = model.Email,
                Sifre = model.Sifre
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // UserRole ekliyoruz
            var userRole = new UserRole
            {
                UserId = newUser.Id,
                RoleId = 2
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        //public async Task<IActionResult> ApplicationList()
        //{
        //    var taleps = await _context.GetAllApplicationsAsync();
        //    return View(taleps);
        //}
        ////
        public async Task<IActionResult> ApplicationList( string searchString)
        {           
            var taleps = _context.Taleps.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                taleps = taleps.Where(t => t.Ad.Contains(searchString) || t.Soyad.Contains(searchString));
            }

            taleps = taleps.OrderByDescending(u => u.CreatedDate);

            var sortedTaleps = await taleps.AsNoTracking().ToListAsync();
            return View(sortedTaleps);
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


