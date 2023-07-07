using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Smart_Invoice.Data;
using Smart_Invoice.Models.Sales;

namespace Smart_Invoice.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    public class SalesInvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesInvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accountant/SalesInvoices
        public async Task<IActionResult> Index()
        {
              return _context.salesInvoices != null ? 
                          View(await _context.salesInvoices.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.salesInvoices'  is null.");
        }

        // GET: Accountant/SalesInvoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.salesInvoices == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.salesInvoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesInvoice == null)
            {
                return NotFound();
            }

            return View(salesInvoice);
        }

        // GET: Accountant/SalesInvoices/Create
        public IActionResult Create()
        {
            SalesInvoice invoice = new SalesInvoice();
            invoice.Customer = new Models.Customer();
            invoice.Notes = "";
            invoice.Products = new List<Models.Invoices.InvoiceItem>();
            invoice.IssueDate = DateTime.Today;
            invoice.DueDate = DateTime.Now;
            return View(invoice);
        }

        // POST: Accountant/SalesInvoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesInvoice salesInvoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesInvoice);
        }

        // GET: Accountant/SalesInvoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.salesInvoices == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.salesInvoices.FindAsync(id);
            if (salesInvoice == null)
            {
                return NotFound();
            }
            return View(salesInvoice);
        }

        // POST: Accountant/SalesInvoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] SalesInvoice salesInvoice)
        {
            if (id != salesInvoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesInvoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesInvoiceExists(salesInvoice.Id))
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
            return View(salesInvoice);
        }

        // GET: Accountant/SalesInvoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.salesInvoices == null)
            {
                return NotFound();
            }

            var salesInvoice = await _context.salesInvoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesInvoice == null)
            {
                return NotFound();
            }

            return View(salesInvoice);
        }

        // POST: Accountant/SalesInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.salesInvoices == null)
            {
                return Problem("Entity set 'ApplicationDbContext.salesInvoices'  is null.");
            }
            var salesInvoice = await _context.salesInvoices.FindAsync(id);
            if (salesInvoice != null)
            {
                _context.salesInvoices.Remove(salesInvoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesInvoiceExists(int id)
        {
          return (_context.salesInvoices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
