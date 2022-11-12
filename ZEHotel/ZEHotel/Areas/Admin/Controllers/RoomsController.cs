using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZEHotel.DAL;
using ZEHotel.Helpers;
using ZEHotel.Models;

namespace ZEHotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public RoomsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Room> rooms = await _db.Rooms.Include(x => x.Customer).Include(x => x.RoomImages)
                .Include(x => x.RoomType).Where(x => !x.IsDeactive).ToListAsync();
            return View(rooms);
        }

        // GET: Admin/Rooms/Create
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _db.Customers.ToListAsync();
            ViewBag.RoomTypes = await _db.RoomTypes.ToListAsync();
            return View();
        }

        // POST: Admin/Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room, int? typeId)
        {
            ViewBag.Customers = await _db.Customers.ToListAsync();
            ViewBag.RoomTypes = await _db.RoomTypes.ToListAsync();
            bool isExist = await _db.Rooms.AnyAsync(x => x.Number == room.Number);
            if (isExist)
            {
                ModelState.AddModelError("Number", "Bu nömrəli otaq artıq mövcuddur!");
                return View();
            }
            if (typeId == null)
            {
                ModelState.AddModelError("FullName", "Zəhmət olmasa otaq üçün bir tip təyin edin.");
                return View();
            }
            RoomType roomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == typeId);
            if (roomType == null)
            {
                ModelState.AddModelError("FullName", "Room Type Error");
                return View();
            }
            if (room.Photos == null)
            {
                ModelState.AddModelError("Photos", "Şəkillər sütunu boş ola bilməz. Zəhmət olmasa bir şəkil faylı seçin!");
                return View();
            }
            List<RoomImage> roomImages = new List<RoomImage>();
            foreach (IFormFile Photo in room.Photos)
            {
                RoomImage roomImage = new RoomImage();
                if (!Photo.IsImage())
                {
                    ModelState.AddModelError("Photos", "Seçdiyiniz fayl şəkil faylı deyil. Zəhmət olmasa şəkil faylı seçin!");
                    return View();
                }
                if (Photo.OlderThreeMb())
                {
                    ModelState.AddModelError("Photos", "Bu şəkil 3MB-dan çoxdur. Zəhmət olmasa daha kiçik ölçülü şəkil faylı seçin!");
                    return View();
                }
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "room");
                roomImage.Image = await Photo.SaveFileAsync(path);
                roomImages.Add(roomImage);
            }
            room.RoomImages = roomImages;
            room.RoomTypeId = (int)typeId;
            await _db.Rooms.AddAsync(room);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        // Get: Admin/Rooms/Edit
        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room room = await _db.Rooms.Include(x => x.RoomImages)
                .Include(x => x.RoomType).FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
            {
                return BadRequest();
            }
            return View(room);
        }

        // POST: Admin/Rooms/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Room room, int? typeId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room dbRoom = await _db.Rooms.Include(x => x.RoomImages)
                .Include(x => x.RoomType).FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbRoom);
            }
            if (typeId == null)
            {
                ModelState.AddModelError("FullName", "RoomType Error");
                return View();
            }
            RoomType dbroomType = await _db.RoomTypes.FirstOrDefaultAsync(x => x.Id == typeId);
            if (dbroomType == null)
            {
                ModelState.AddModelError("Name", "RoomType Error");
                return View();
            }
            if (room.Photos != null)
            {
                List<RoomImage> roomImages = new List<RoomImage>();
                foreach (IFormFile Photo in room.Photos)
                {
                    RoomImage roomImage = new RoomImage();
                    if (!Photo.IsImage())
                    {
                        ModelState.AddModelError("Photos", "Zəhmət olmasa şəkil faylı seçin!");
                        return View();
                    }
                    if (Photo.OlderThreeMb())
                    {
                        ModelState.AddModelError("Photos", "Şəkil 3MB-dan çoxdur. Zəhmət olmasa daha kiçik ölçülü fayl seçin!");
                        return View();
                    }
                    string path = Path.Combine(_env.WebRootPath, "assets", "images", "room");
                    roomImage.Image = await Photo.SaveFileAsync(path);
                    roomImages.Add(roomImage);
                }
                dbRoom.RoomImages.AddRange(roomImages);
            }
            dbRoom.Number = room.Number;
            dbroomType.Id = (int)typeId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        // GET: Admin/Rooms/Details
        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room dbRoom = await _db.Rooms.Include(x => x.RoomImages).Include(x => x.Customer).Include(x => x.RoomType).FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }
            return View(dbRoom);
        }
        #endregion

        // GET: Admin/Rooms/Delete
        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room dbRoom = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }
            return View(dbRoom);
        }

        // POST: Admin/Rooms/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room dbRoom = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }
            dbRoom.IsDeactive = true;
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
            Room dbRoom = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }
            if (dbRoom.IsDeactive)
            {
                dbRoom.IsDeactive = false;
            }
            else
            {
                dbRoom.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region DeleteImage
        public async Task<IActionResult> DeleteImage(int? imgId)
        {
            if (imgId == null)
            {
                return NotFound();
            }
            RoomImage roomImage = await _db.RoomImages.FirstOrDefaultAsync(x => x.Id == imgId);
            if (roomImage == null)
            {
                return BadRequest();
            }
            int count = _db.RoomImages.Count(x => x.RoomId == roomImage.RoomId);
            if (count > 1)
            {
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "room", roomImage.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _db.RoomImages.Remove(roomImage);
                await _db.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
