using BizLand.Areas.Admin.ViewModels;
using BizLand.DAL;
using BizLand.Models;
using BizLand.Utilities.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public TeamController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _db.Teams.Include(x => x.Position).ToListAsync();
            return View(teams);
        }

        public async Task<IActionResult> Create()
        {
            CreateTeamVM create = new CreateTeamVM
            {
                Positions = await _db.Positions.ToListAsync(),

            };
            return View(create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM create)
        {
            if (!ModelState.IsValid)
            {
                create.Positions = await _db.Positions.ToListAsync();
                return View(create);
            }

            bool result = await _db.Teams.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.Trim().ToLower());

            if (result)
            {
                create.Positions = await _db.Positions.ToListAsync();
                ModelState.AddModelError("Name", "is exists");
                return View(create);
            }

            if (create.Photo.ValidateType())
            {
                create.Positions = await _db.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "not valid");
                return View(create);
            }
            if (create.Photo.ValidateSize(10))
            {
                create.Positions = await _db.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "max 10mb");
                return View(create);
            }

            Team team = new Team
            {
                Name = create.Name,
                TwitLink = create.TwitLink,
                InstaLink = create.InstaLink,
                LinkedLink = create.LinkedLink,
                FaceLink = create.FaceLink,
                PositionId = create.PositionId,
                Image = await create.Photo.CreateFile(_env.WebRootPath, "assets", "img")
            };

            await _db.Teams.AddAsync(team);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Team team = await _db.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team == null) return NotFound();

            UpdateTeamVM update = new UpdateTeamVM
            {
                Name = team.Name,
                InstaLink = team.InstaLink,
                LinkedLink = team.LinkedLink,
                FaceLink = team.FaceLink,
                PositionId = team.PositionId,
                Positions = await _db.Positions.ToListAsync(),
            };

            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateTeamVM update)
        {
            if (!ModelState.IsValid)
            {
                update.Positions = await _db.Positions.ToListAsync();
                return View(update);
            }
            Team team = await _db.Teams.FirstOrDefaultAsync(x=>x.Id==id);
            if (team == null) return NotFound();

            bool result = await _db.Teams.AnyAsync(x=>x.Name.Trim().ToLower()==update.Name.Trim().ToLower() && x.Id==id);

            if (result)
            {
                update.Positions=await _db.Positions.ToListAsync();
                ModelState.AddModelError("Name","is exists");
                return View(update);
            }

            if(update.Photo is not null)
            {
                if (update.Photo.ValidateType())
                {
                    update.Positions = await _db.Positions.ToListAsync();
                    ModelState.AddModelError("Name", "not valid");
                    return View(update);
                }
                if (update.Photo.ValidateSize(10))
                {
                    update.Positions = await _db.Positions.ToListAsync();
                    ModelState.AddModelError("Name", "is exists");
                    return View(update);
                }
                team.Image.DeleteFile(_env.WebRootPath,"assets","img");
                team.Image=await update.Photo.CreateFile(_env.WebRootPath, "assets", "img");
            }
            team.Name=update.Name;
            team.FaceLink=update.FaceLink;
            team.LinkedLink=update.LinkedLink;
            team.TwitLink=update.TwitLink;
            team.InstaLink=update.InstaLink;
            team.PositionId=update.PositionId;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Team team= await _db.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) return NotFound();
            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
