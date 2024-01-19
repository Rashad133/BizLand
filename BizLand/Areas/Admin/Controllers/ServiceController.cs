using BizLand.Areas.Admin.ViewModels;
using BizLand.DAL;
using BizLand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _db;
        public ServiceController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Service> services = await _db.Services.ToListAsync();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM create)
        {
            if (!ModelState.IsValid)
            {
                return View(create);
            }

            bool result = await _db.Services.AnyAsync(x => x.Title.Trim().ToLower() == create.Title.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "is exists");
                return View(create);
            }
            Service service = new Service
            {
                Title = create.Title,
                SubTitle = create.Subtitle,
                Icon = create.Icon,
            };

            await _db.Services.AddAsync(service);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Service service = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service == null) return NotFound();

            UpdateServiceVM update = new UpdateServiceVM
            {
                Title = service.Title,
                Subtitle = service.SubTitle,
                Icon = service.Icon,
            };
            return View(update);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateServiceVM update)
        {
            if (!ModelState.IsValid) return View(update);

            Service service = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service == null) return NotFound();

            bool result = await _db.Services.AnyAsync(x => x.Title.Trim().ToLower() == update.Title.Trim().ToLower() && x.Id == id);

            if (result)
            {
                ModelState.AddModelError("Name", "is exists");
                return View(update);
            }

            service.Title = update.Title;
            service.SubTitle = update.Subtitle; 
            service.Icon = update.Icon;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id<=0) return BadRequest();
            Service service= await _db.Services.FirstOrDefaultAsync(y => y.Id == id);
            if (service == null) return NotFound();

            _db.Services.Remove(service);
            return RedirectToAction(nameof(Index));
        }

    }
}
