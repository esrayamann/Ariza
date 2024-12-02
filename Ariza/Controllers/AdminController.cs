using Ariza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ariza.Controllers
{
    public class AdminController : Controller
    {
        //        private readonly UserService _userService;

        //        public AdminController(UserService userService)
        //        {
        //            _userService = userService;
        //        }

        //        [HttpGet]
        //        public async Task<IActionResult> UpdateRole(int id)
        //        {
        //            var user = await _userService.GetUserByIdAsync(id);
        //            if (user == null)//yeni
        //            {
        //                return NotFound("Kullanıcı bulunamadı.");
        //            }

        //            //var allRoles = await _userService.AllRolesAsync();
        //            //if (allRoles == null || !allRoles.Any())//yeni
        //            //{
        //            //    return NotFound("Roller bulunamadı.");
        //            //}
        //            //var userRoles = user.UserRoles ?? new List<UserRole>();//

        //            //var model = new UpdateRoleViewModel
        //            //{
        //            //    UserId = id,
        //            //    AllRoles = allRoles.ToList(),
        //            //    UserRoles = user.UserRoles.Select(ur => ur.RoleId).ToList()//burada olabilir hata
        //            //};

        //            return View(/*model*/);
        //        }

        //        //[HttpPost]
        //        //public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        //        //{
        //        //    if (ModelState.IsValid)
        //        //    {
        //        //        await _userService.UpdateUserRolesAsync(model.UserId, model.SelectedRoles);
        //        //        return RedirectToAction("KullanıcıListesi");
        //        //    }
        //        //    return View(model);
        //        //}
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}


