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
    public class CustomersController : Controller
    {
        private readonly AppDbContext _db;
        public CustomersController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Customer> customers = await _db.Customers.Where(x => !x.IsDeactive).ToListAsync();
            return View(customers);
        }

        #region Create
        // GET: Admin/Customers/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Admin/Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Customers.AnyAsync(x => x.Phone == customer.Phone && x.Mail == customer.Mail);
            if (isExist)
            {
                ModelState.AddModelError("Phone", "Bu nömrə artıq istifadə olunub!");
                return View();
            }
            if (isExist)
            {
                ModelState.AddModelError("Mail", "Bu mail adresi artıq istifadə olunub!");
                return View();
            }
            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        //GET: Admin/Customers/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer dbCustomer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCustomer == null)
            {
                return BadRequest();
            }
            return View(dbCustomer);
        }
        #endregion

        #region Edit
        // GET: Admin/Customers/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer dbCustomers = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCustomers == null)
            {
                return BadRequest();
            }
            return View(dbCustomers);
        }
        // POST: Admin/Customers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Customer customer)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer dbCustomers = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCustomers == null)
            {
                return BadRequest();
            }
            bool isExist = await _db.Customers.AnyAsync(x => x.Mail == customer.Mail && x.Phone == customer.Phone && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Mail", "Bu mail adresi artıq istifadə olunub!");
                return View();
            }
            if (isExist)
            {
                ModelState.AddModelError("Phone", "Bu nömrə artıq istifadə olunub!");
                return View();
            }
            dbCustomers.Name = customer.Name;
            dbCustomers.Surname = customer.Surname;
            dbCustomers.Mail = customer.Mail;
            dbCustomers.Phone = customer.Phone;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        // GET: Admin/Customers/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer dbCustomer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCustomer == null)
            {
                return BadRequest();
            }
            return View(dbCustomer);
        }

        // POST: Admin/Customers/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer dbCustomer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCustomer == null)
            {
                return BadRequest();
            }
            dbCustomer.IsDeactive = true;
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
            Customer dbCustomer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCustomer == null)
            {
                return BadRequest();
            }
            if (dbCustomer.IsDeactive)
            {
                dbCustomer.IsDeactive = false;
            }
            else
            {
                dbCustomer.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
