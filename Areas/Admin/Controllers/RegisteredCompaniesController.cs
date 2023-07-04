using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Smart_Invoice.Data;
using Smart_Invoice.Models.Registered_Companies;

namespace Smart_Invoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegisteredCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisteredCompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/RegisteredCompanies
        public async Task<IActionResult> Index()
        {
              return _context.RegisteredCompanies != null ? 
                          View(await _context.RegisteredCompanies.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.RegisteredCompanies'  is null.");
        }

        // GET: Admin/RegisteredCompanies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.RegisteredCompanies == null)
            {
                return NotFound();
            }

            var registeredCompany = await _context.RegisteredCompanies
                .FirstOrDefaultAsync(m => m.CompanyCode == id);
            if (registeredCompany == null)
            {
                return NotFound();
            }

            return View(registeredCompany);
        }

        // GET: Admin/RegisteredCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/RegisteredCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyCode,CompanyName,CompanyPhone,CompanyAddress,GovCompanyRegistration")] RegisteredCompany registeredCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registeredCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registeredCompany);
        }

        // GET: Admin/RegisteredCompanies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.RegisteredCompanies == null)
            {
                return NotFound();
            }

            var registeredCompany = await _context.RegisteredCompanies.FindAsync(id);
            if (registeredCompany == null)
            {
                return NotFound();
            }
            return View(registeredCompany);
        }

        // POST: Admin/RegisteredCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CompanyCode,CompanyName,CompanyPhone,CompanyAddress,GovCompanyRegistration")] RegisteredCompany registeredCompany)
        {
            if (id != registeredCompany.CompanyCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registeredCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegisteredCompanyExists(registeredCompany.CompanyCode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(registeredCompany);
        }

        // GET: Admin/RegisteredCompanies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.RegisteredCompanies == null)
            {
                return NotFound();
            }

            var registeredCompany = await _context.RegisteredCompanies
                .FirstOrDefaultAsync(m => m.CompanyCode == id);
            if (registeredCompany == null)
            {
                return NotFound();
            }

            return View(registeredCompany);
        }

        // POST: Admin/RegisteredCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.RegisteredCompanies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RegisteredCompanies'  is null.");
            }
            var registeredCompany = await _context.RegisteredCompanies.FindAsync(id);
            if (registeredCompany != null)
            {
                _context.RegisteredCompanies.Remove(registeredCompany);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegisteredCompanyExists(long id)
        {
          return (_context.RegisteredCompanies?.Any(e => e.CompanyCode == id)).GetValueOrDefault();
        }
    }
}
