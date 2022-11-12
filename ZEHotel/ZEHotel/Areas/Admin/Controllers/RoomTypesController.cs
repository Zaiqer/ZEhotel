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
    public class RoomTypesController : Controller
    {
        public readonly AppDbContext _db;
        public RoomTypesController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<RoomType> roomTypes = _db.RoomTypes.Where(x => !x.IsDeactive).ToList();
            return View(roomTypes);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomType roomType)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.RoomTypes.AnyAsync(x => x.Name == roomType.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adlı otaq tipi artıq mövcuddur!");
                return View();
            }
            await _db.RoomTypes.AddAsync(roomType);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoomType dbroomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (dbroomType == null)
            {
                return BadRequest();
            }
            return View(dbroomType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, RoomType roomType)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoomType dbroomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (dbroomType == null)
            {
                return BadRequest();
            }
            bool isExist = await _db.RoomTypes.AnyAsync(x => x.Name == roomType.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adlı otaq tipi artıq mövcuddur!");
                return View();
            }
            dbroomType.Name = roomType.Name;
            dbroomType.Description = roomType.Description;
            dbroomType.Capacity = roomType.Capacity;
            dbroomType.Price = roomType.Price;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoomType dbroomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (dbroomType == null)
            {
                return BadRequest();
            }
            return View(dbroomType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoomType dbroomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (dbroomType == null)
            {
                return BadRequest();
            }
            dbroomType.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Rooms = await _db.Rooms.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            RoomType dbroomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (dbroomType == null)
            {
                return BadRequest();
            }
            return View(dbroomType);
        }
        #endregion

        #region Activation
        public async Task<IActionResult> Activation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoomType dbroomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (dbroomType == null)
            {
                return BadRequest();
            }
            if (dbroomType.IsDeactive)
            {
                dbroomType.IsDeactive = false;
            }
            else
            {
                dbroomType.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
