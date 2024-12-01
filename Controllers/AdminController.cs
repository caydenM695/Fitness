using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Fitness.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fitness.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        public IActionResult Test()
        {
            return View();
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        //public async Task<IActionResult> MakeAdmin()
        //{
        //    if (!await _roleManager.RoleExistsAsync("Admin"))
        //    {
        //        var adminRole = new IdentityRole("Admin");
        //        await _roleManager.CreateAsync(adminRole);
        //    }

        //    var user = await _userManager.FindByIdAsync("f1f1ffaf-bfa7-423b-88e2-17bc12513a48");
        //    var result = await _userManager.AddToRoleAsync(user, "Admin");

        //    return View();
        //}
    }
}
