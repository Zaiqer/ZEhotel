using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZEHotel.DAL;
using ZEHotel.Helpers;
using ZEHotel.Models;

namespace ZEHotel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public EmployeesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: Admin/Employees
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _db.Employee.Where(x => !x.IsDeactive).ToListAsync();
            return View(employees);
        }

        // GET: Admin/Employees/Create
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            return View();
        }

        // POST: Admin/Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, int? posId)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (posId == null)
            {
                ModelState.AddModelError("FullName", "Zəhmət olmasa işçi üçün bir vəzifə təyin edin.");
                return View();
            }
            Position position = await _db.Positions.FirstOrDefaultAsync(x => x.Id == posId);
            if (position == null)
            {
                ModelState.AddModelError("FullName", "Position Error");
                return View();
            }
            if (employee.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkillər sütunu boş ola bilməz. Zəhmət olmasa bir şəkil faylı seçin!");
                return View();
            }
            if (!employee.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil faylı seçin!");
                return View();
            }
            if (employee.Photo.OlderThreeMb())
            {
                ModelState.AddModelError("Photo", "Bu şəkil 3MB-dan çoxdur. Zəhmət olmasa daha kiçik ölçülü şəkil faylı seçin!");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets", "images", "employee");
            employee.Image = await employee.Photo.SaveFileAsync(path);
            employee.PositionId = (int)posId;
            await _db.Employee.AddAsync(employee);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        // GET: Admin/Employees/Edit/5
        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Employee dbEmployee = await _db.Employee.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            return View(dbEmployee);
        }

        // POST: Admin/Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Employee employee, int? posId)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Employee dbEmployee = await _db.Employee.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbEmployee);
            }
            if (posId == null)
            {
                ModelState.AddModelError("FullName", "Position Error");
                return View();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == posId);
            if (dbPosition == null)
            {
                ModelState.AddModelError("FullName", "Position Error");
                return View();
            }
            if (employee.Photo != null)
            {
                if (!employee.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image file!");
                    return View(dbEmployee);
                }
                if (employee.Photo.OlderThreeMb())
                {
                    ModelState.AddModelError("Photo", "Photo more than 3 MB!");
                    return View(dbEmployee);
                }
                string path = Path.Combine(_env.WebRootPath, "assets", "images", "emplyee");
                if (System.IO.File.Exists(Path.Combine(path, dbEmployee.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, dbEmployee.Image));
                }
                dbEmployee.Image = await employee.Photo.SaveFileAsync(path);
            }
            dbEmployee.FullName = employee.FullName;
            dbEmployee.Email = employee.Email;
            dbEmployee.Phone = employee.Phone;
            dbEmployee.Salary = employee.Salary;
            dbEmployee.PositionId = (int)posId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        // GET: Admin/Employees/Details/5
        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee dbEmployee = await _db.Employee.Include(x => x.Position).FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            return View(dbEmployee);
        }
        #endregion

        // GET: Admin/Employees/Delete/5
        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee dbEmployee = await _db.Employee.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            return View(dbEmployee);
        }

        // POST: Admin/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee dbEmployee = await _db.Employee.FirstOrDefaultAsync(s => s.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            dbEmployee.IsDeactive = true;
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
            Employee dbEmployee = await _db.Employee.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployee == null)
            {
                return BadRequest();
            }
            if (dbEmployee.IsDeactive)
            {
                dbEmployee.IsDeactive = false;
            }
            else
            {
                dbEmployee.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
