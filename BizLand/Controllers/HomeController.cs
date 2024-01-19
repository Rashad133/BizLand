using BizLand.DAL;
using BizLand.Models;
using BizLand.Utilities.Enums;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(AppDbContext db,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _db.Teams.Include(x=>x.Position).ToListAsync();
            List<Service> services =await _db.Services.ToListAsync();

            HomeVM vm = new HomeVM
            {
                Teams = teams,
                Services = services
            };
            
            return View(vm);
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
