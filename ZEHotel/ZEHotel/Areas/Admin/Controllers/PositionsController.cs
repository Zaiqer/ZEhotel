using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZEHotel.DAL;
using ZEHotel.Models;

namespace ZEHotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionsController : Controller
    {

        private readonly AppDbContext _db;
        public PositionsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Employees = await _db.Employee.ToListAsync();
            List<Position> positions = await _db.Positions.Include(x => x.Employees).Where(x => !x.IsDeactive).ToListAsync();
            return View(positions);
        }

        #region Create
        // GET: Admin/Positions/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Admin/Positions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Positions.AnyAsync(x => x.Name == position.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adlı vəzifə artıq mövcuddur!");
                return View();
            }
            await _db.Positions.AddAsync(position);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        // GET: Admin/Positions/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPositions = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPositions == null)
            {
                return BadRequest();
            }
            return View(dbPositions);
        }
        // POST: Admin/Positions/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Position position)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPositions = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPositions == null)
            {
                return BadRequest();
            }
            bool isExist = await _db.Positions.AnyAsync(x => x.Name == position.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adlı vəzifə artıq mövcuddur!");
                return View();
            }
            dbPositions.Name = position.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        // GET: Admin/Positions/Details
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Employees = await _db.Employee.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            return View(dbPosition);
        }
        #endregion

        #region Delete
        // GET: Admin/Positions/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            return View(dbPosition);
        }

        // POST: Admin/Positions/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            dbPosition.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Activation
        public async Task<IActionResult> Activation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPostion = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPostion == null)
            {
                return BadRequest();
            }
            if (dbPostion.IsDeactive)
            {
                dbPostion.IsDeactive = false;
            }
            else
            {
                dbPostion.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
