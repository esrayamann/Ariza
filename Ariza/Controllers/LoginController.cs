using Ariza.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ariza.Controllers
{
    public class LoginController : Controller
    {
        private readonly Context _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginController(Context context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var normalizedEmail = model.Email.Trim().ToLowerInvariant();
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(x => x.Email == normalizedEmail);

            if (user is not null)
            {
                var verification = _passwordHasher.VerifyHashedPassword(user, user.Sifre, model.Password);
                if (verification == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
                    return View(model);
                }

                if (verification == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.Sifre = _passwordHasher.HashPassword(user, model.Password);
                    await _context.SaveChangesAsync();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                foreach (var role in user.UserRoles.Select(ur => ur.Role.Rol))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                if (user.UserRoles.Any(ur => ur.Role.Rol == RoleNames.Admin))
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("MyApplications", "Talep");
            }

            ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı.");
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
    }
}
