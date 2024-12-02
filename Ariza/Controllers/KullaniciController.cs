using Microsoft.AspNetCore.Mvc;

namespace Ariza.Controllers
{
    public class KullaniciController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
