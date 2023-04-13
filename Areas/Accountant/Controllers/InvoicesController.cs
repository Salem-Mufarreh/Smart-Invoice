using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.DocumentAI.V1;
using Google.Apis.Storage.v1;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Smart_Invoice.Data;
using Smart_Invoice.Models;
using Smart_Invoice.Utility;
using Google.Cloud.Storage.V1;
using System.Text;
using Smart_Invoice.Areas.Identity.Pages.Account;
using NuGet.Configuration;

namespace Smart_Invoice.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoginModel> _logger;

        public InvoicesController(ApplicationDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Accountant/Invoices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoices.Include(i => i.Customer);
            List<Invoice> invoices = await applicationDbContext.ToListAsync();
            return View(invoices);
        }

        // GET: Accountant/Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }
        [HttpGet]
        // GET: Accountant/Invoices/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Address");

            
            return View();
        }

        // POST: Accountant/Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file)
        {
            // Extract the Invoice using API call
            Google.Cloud.DocumentAI.V1.Document document = await ExtractInvoice(file);
            // Upload the invoice to the cloud bucket 
            var uploaded = CloudUpload(file);
            
            /* if (ModelState.IsValid)
             {
                 _context.Add(invoice);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Address", invoice.CustomerId);*/
            return View();
        }
       
 

        // GET: Accountant/Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Address", invoice.CustomerId);
            return View(invoice);
        }

        // POST: Accountant/Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,InvoiceNumber,InvoiceDate,DueDate,TotalAmount,CurrencyCode,PaymentStatus,PaymentDate,ExchangeRate,EffectiveDate,TaxPercentage,TaxTotal")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Address", invoice.CustomerId);
            return View(invoice);
        }

        // GET: Accountant/Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Accountant/Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
          return (_context.Invoices?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #region APICalls
        public async Task<Boolean> CloudUpload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var storage = StorageClient.Create();

                    // Converting the file to byte[] to stram it to cloud 
                    byte[] fileBytes;
                    using (var stream = file.OpenReadStream())
                    using (var ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }
                    // Generate a unique name for the file using a GUID
                    using (var ms = new MemoryStream(fileBytes))
                    {
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        await storage.UploadObjectAsync("samrtinvoice_bucket", fileName, "application/octet-stream", ms);
                    }
                    // Return a success response
                    return true;
                }
                catch (Exception ex)
                {
                    // Handle the exception and return an error response
                    _logger.LogTrace(StatusCodes.Status500InternalServerError, ex.Message);
                    return false ;
                }
            }
            else
            {
                // Return a bad request response
                return false;
            }
        }
        public async Task<Google.Cloud.DocumentAI.V1.Document> ExtractInvoice(
            IFormFile file,
            
            string projectId = "document-ea-369818",
            string locationId = "eu",
            string processorId = "578778ce00c7a18",
            string mimeType = "application/pdf")
        {
            // Create client
            var client = new DocumentProcessorServiceClientBuilder
            {
                Endpoint = $"{locationId}-documentai.googleapis.com"
            }.Build();

            // Read in local file
            using var fileStream = file.OpenReadStream();
            var rawDocument = new RawDocument
            {
                Content = ByteString.FromStream(fileStream),
                MimeType = mimeType
            };

            // Initialize request argument(s)
            var request = new ProcessRequest
            {
                Name = ProcessorName.FromProjectLocationProcessor(projectId, locationId, processorId).ToString(),
                RawDocument = rawDocument
            };

            // Make the request
            var response = client.ProcessDocument(request);

            var document = response.Document;
            return document;
        }

        #endregion
    }
}
