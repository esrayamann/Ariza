using Ariza.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ariza.Controllers
{
    public class LoginController : Controller
    {
        private readonly Context _context;

        public LoginController(Context context)
        {
            _context = context;
        }
        [HttpGet]

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(User p)
        {
            var user = _context.Users
               .Include(u => u.UserRoles)
               .ThenInclude(ur => ur.Role)
               .FirstOrDefault(x => x.Email == p.Email && x.Sifre == p.Sifre);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)
                    //new Claim(ClaimTypes.Email, p.Email)
                };

                foreach (var role in user.UserRoles.Select(ur => ur.Role.Rol))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                //var useridentity = new ClaimsIdentity(claims, "CookieAuthentication");
                //ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);//
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                if (user.UserRoles.Any(ur => ur.RoleId == 1))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.UserRoles.Any(ur => ur.RoleId == 2))
                {
                    return RedirectToAction("Index", "Kullanici");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(p);
        }
    }
}
