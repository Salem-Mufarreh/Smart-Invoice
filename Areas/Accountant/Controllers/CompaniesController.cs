using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Smart_Invoice.Data;
using Smart_Invoice.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Smart_Invoice.Models.Invoices;
using Newtonsoft.Json;

namespace Smart_Invoice.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accountant/Companies
        public async Task<IActionResult> Index()
        {
              return _context.Companies != null ? 
                          View(await _context.Companies.Include(a=>a.person).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Companies'  is null.");
        }

        [HttpGet]
        // GET: Accountant/Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.Include(c => c.person)
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Accountant/Companies/Create
        public IActionResult Create()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Contacts, "ContactPersonId", "Name");
            Company company = new Company();
            // TODO
            if(TempData["viewModel"] != null)
            {
                InvoiceViewModel invoiceViewModel = JsonConvert.DeserializeObject<InvoiceViewModel>(TempData["viewModel"].ToString());
                Product_Invoice product = invoiceViewModel.ProductInvoice;
                if (product != null)
                {
                    company = product.Company;
                    
                }
            }
            
            return View(company);
        }

        // POST: Accountant/Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Company_Name,Company_Name_Normilized,Company_Name_English,Address,Company_License_Registration_Number,Phone,Email,person")] Company company)
        {   
            var contact = _context.Contacts.Where(x => x.ContactPersonId.Equals(company.person.ContactPersonId)).FirstOrDefault();
            company.person = contact;
            ModelState.Clear();
            TryValidateModel(company);
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                var nextView = HttpContext.Session.GetString("NextView");
                if (nextView != null)
                {
                    try
                    {
                        HttpContext.Session.Remove("NextView");
                        return RedirectToAction("Edit", "Invoices");
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Accountant/Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.Include(a => a.person).FirstOrDefaultAsync(c => c.CompanyId.Equals(id));
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Accountant/Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Company_Name,Company_Name_English,Address,Company_License_Registration_Number,Phone,Email,person")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
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
            return View(company);
        }

        // GET: Accountant/Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Accountant/Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Companies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Companies'  is null.");
            }
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
          return (_context.Companies?.Any(e => e.CompanyId == id)).GetValueOrDefault();
        }

        #region API Calls

        [HttpGet]
        public async Task<IActionResult> InitCompanyPartial()
        {
            InvoiceViewModel model = JsonConvert.DeserializeObject<InvoiceViewModel>(HttpContext.Session.GetString("ViewModel"));
            if (model != null)
            {
                Product_Invoice invoice = model.ProductInvoice;
                if (invoice != null)
                {
                    return PartialView("CreateCompanyPartial", invoice.Company);
                }
                return Problem();
            }
            else
            {
                return Problem();
            }

        }

        [HttpPost]
        public  Task<IActionResult> SubmitCompany([FromBody] Company company)
        {
            company.person = _context.Contacts.Where(c => c.Name.Equals("Salem")).FirstOrDefault();
            if (ModelState.IsValid)
            {
                 _context.Companies.Add(company);
                 _context.SaveChanges();
                return Task.FromResult<IActionResult>(Ok());
            }
            else
            {
                return Task.FromResult<IActionResult>(NotFound());
            }
        }
        [HttpGet]
        public JsonResult GetCustomerList()
        {
            // Retrieve the customer list from your data source
            var customers = _context.Customers.ToList();

            // Transform the customer data into a format suitable for JSON serialization
          

            // Return the customer data as JSON
            return Json(customers);
        }

        #endregion
    }

}
