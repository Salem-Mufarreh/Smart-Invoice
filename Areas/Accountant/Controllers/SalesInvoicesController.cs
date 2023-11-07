using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Smart_Invoice.Data;
using Smart_Invoice.Models;
using Smart_Invoice.Models.Invoices;
using Smart_Invoice.Models.Products;
using Smart_Invoice.Models.Sales;
using Document = iText.Layout.Document;

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
            else
            {
                List<GinvoiceProp> items = new List<GinvoiceProp>();
                items.AddRange((IEnumerable<GinvoiceProp>)_context.ginvoices.Where(p => p.SalesInvoiceId == id).ToList());
                salesInvoice.Products =items ;
            }
            

            return View(salesInvoice);
        }

        // GET: Accountant/SalesInvoices/Create
        public IActionResult Create()
        {
            SalesInvoice invoice = new SalesInvoice();
            invoice.Invoice_number = "23/00002585";
            invoice.Customer = new Models.Customer();
            invoice.Products = new List<GinvoiceProp>();
            invoice.IssueDate = DateTime.Today;
            invoice.DueDate = DateTime.Now.AddMonths(1);
            invoice.Notes = "Thank you for your business. We appreciate your prompt payment.";
            return View(invoice);
        }

        // POST: Accountant/SalesInvoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesInvoice salesInvoice)
        {
            ModelState.ClearValidationState(key:"Customer");
            ModelState.ClearValidationState(key: "Product");
            ModelState.ClearValidationState(key: "Discount");
            
            salesInvoice.Customer = _context.Customers.Where(c => c.CustomerId.Equals(salesInvoice.Customer.CustomerId)).FirstOrDefault();
            for (var i=0; i<salesInvoice.Products.Count;i++ )
            {
                salesInvoice.Products[i].Product = _context.Products.Where(p => p.ProductId.Equals(salesInvoice.Products[i].productId)).FirstOrDefault();
                salesInvoice.Products[i].Name = salesInvoice.Products[i].Product.Name;
                if (salesInvoice.Products[i].Discount == null)
                {

                }
            }

            bool isvalid = TryValidateModel(salesInvoice);
            if (ModelState.IsValid) 
            {
                GenerateSaleInvoice(salesInvoice);
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
                _context.ginvoices.RemoveRange(_context.ginvoices.Where(g => g.SalesInvoiceId.Equals(id)));
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesInvoiceExists(int id)
        {
          return (_context.salesInvoices?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public string GenerateSaleInvoice(SalesInvoice salesInvoice)
        {
            var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // Set up fonts
            var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            // Header section
            Paragraph header = new Paragraph("Company Details").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20);
            document.Add(header);
            Paragraph subheader = new Paragraph("PDF CREATED USING ASP.NET C# WITH iTExT7 LIBRARY").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10);
            document.Add(subheader);
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);



            // Invoice details section
            var invoiceDetailsTable = new Table(UnitValue.CreatePercentArray(new[] { 1f, 1f }))
                .SetMaxWidth(200)
                .SetBorder(Border.NO_BORDER)
                .SetMarginBottom(20);

            invoiceDetailsTable.AddCell(CreateCell("Invoice Number:", boldFont, TextAlignment.LEFT));
            invoiceDetailsTable.AddCell(CreateCell(salesInvoice.Invoice_number, regularFont, TextAlignment.LEFT));
            invoiceDetailsTable.AddCell(CreateCell("Issue Date:", boldFont, TextAlignment.LEFT));
            invoiceDetailsTable.AddCell(CreateCell(salesInvoice.IssueDate.ToString(), regularFont, TextAlignment.LEFT));
            invoiceDetailsTable.AddCell(CreateCell("Due Date:", boldFont, TextAlignment.LEFT));
            invoiceDetailsTable.AddCell(CreateCell(salesInvoice.DueDate.ToString(), regularFont, TextAlignment.LEFT));
     

            document.Add(invoiceDetailsTable);

            // Customer details section
            var customerDetailsTable = new Table(UnitValue.CreatePercentArray(new[] { 1f, 1f }))
                .UseAllAvailableWidth()
                .SetMarginBottom(20);

            customerDetailsTable.AddCell(CreateCell("Customer Name:", boldFont, TextAlignment.LEFT));
            customerDetailsTable.AddCell(CreateCell(salesInvoice.Customer.CustomerName, regularFont, TextAlignment.LEFT));
            customerDetailsTable.AddCell(CreateCell("Address:", boldFont, TextAlignment.LEFT));
            customerDetailsTable.AddCell(CreateCell(salesInvoice.Customer.Address, regularFont, TextAlignment.LEFT));
            customerDetailsTable.AddCell(CreateCell("Registered Tax Number:", boldFont, TextAlignment.LEFT));
            customerDetailsTable.AddCell(CreateCell(salesInvoice.Customer.Store_Tax_Number, regularFont, TextAlignment.LEFT));

            document.Add(customerDetailsTable);

            // Items purchased section
            var itemsPurchasedTable = new Table(UnitValue.CreatePercentArray(new[] { 1f, 3f, 1f, 1f, 1f, 1f }))
                .UseAllAvailableWidth();

            itemsPurchasedTable.AddHeaderCell(CreateHeaderCell("Product Id", boldFont, TextAlignment.CENTER));
            itemsPurchasedTable.AddHeaderCell(CreateHeaderCell("Product Name", boldFont, TextAlignment.CENTER));
            itemsPurchasedTable.AddHeaderCell(CreateHeaderCell("Quantity", boldFont, TextAlignment.CENTER));
            itemsPurchasedTable.AddHeaderCell(CreateHeaderCell("Unit Price", boldFont, TextAlignment.CENTER));
            itemsPurchasedTable.AddHeaderCell(CreateHeaderCell("Discount", boldFont, TextAlignment.CENTER));
            itemsPurchasedTable.AddHeaderCell(CreateHeaderCell("Total", boldFont, TextAlignment.CENTER));

          

            foreach (var product in salesInvoice.Products)
            {
                itemsPurchasedTable.AddCell(CreateCell(product.productId.ToString(), regularFont, TextAlignment.CENTER));
                itemsPurchasedTable.AddCell(CreateCell(product.Name, regularFont, TextAlignment.LEFT));
                itemsPurchasedTable.AddCell(CreateCell(product.Quantity.ToString(), regularFont, TextAlignment.CENTER));
                itemsPurchasedTable.AddCell(CreateCell(product.UnitPrice.ToString("0.00"), regularFont, TextAlignment.RIGHT));
                itemsPurchasedTable.AddCell(CreateCell(product.Discount.ToString("%.00"), regularFont, TextAlignment.RIGHT));
                itemsPurchasedTable.AddCell(CreateCell(product.Total.ToString("0.00"), regularFont, TextAlignment.RIGHT));
            }

            document.Add(itemsPurchasedTable);

            // Footer section
            var footerTable = new Table(UnitValue.CreatePercentArray(new[] { 1f, 1f }))
                .UseAllAvailableWidth()
                .SetAutoLayout()
                .SetMarginTop(20);

            footerTable.AddCell(CreateCell("Subtotal:", boldFont, TextAlignment.RIGHT));
            footerTable.AddCell(CreateCell(salesInvoice.SubTotal.ToString("0.00"), regularFont, TextAlignment.RIGHT));
            footerTable.AddCell(CreateCell("Tax:", boldFont, TextAlignment.RIGHT));
            footerTable.AddCell(CreateCell(salesInvoice.Tax.ToString("0.00"), regularFont, TextAlignment.RIGHT));
            footerTable.AddCell(CreateCell("Total:", boldFont, TextAlignment.RIGHT));
            footerTable.AddCell(CreateCell(salesInvoice.Total.ToString("0.00"), regularFont, TextAlignment.RIGHT));


            document.Add(footerTable);
            Paragraph noteParagraph = new Paragraph(salesInvoice.Notes);
            document.Add(noteParagraph);

            // Close the document
            document.Close();
            var base64 = Convert.ToBase64String(memoryStream.ToArray());

            // Pass the Base64 string to the view
            ViewBag.PdfContent = base64;
            ViewData["PdfContent"] = base64;
            HttpContext.Session.SetString("PdfContent", base64.ToString());
            // Return the PDF document as a file
            return base64;
        }

        private Cell CreateHeaderCell(string content, PdfFont font, TextAlignment alignment)
        {
            var cell = new Cell()
                .Add(new Paragraph(content).SetFont(font).SetFontSize(12))
                .SetTextAlignment(alignment)
                .SetPadding(5)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);

            return cell;
        }

        private Cell CreateCell(string content, PdfFont font, TextAlignment alignment)
        {
            var cell = new Cell()
                .Add(new Paragraph(content).SetFont(font).SetFontSize(10))
                .SetTextAlignment(alignment)
                .SetPadding(5);

            return cell;
        }

        [HttpGet]
        public JsonResult ReturnInvoice()
        {
            var pdf = HttpContext.Session.GetString("PdfContent");
            HttpContext.Session.Remove("PdfContent");
            
            return Json(pdf);
        }
        [HttpGet]
        public JsonResult GetInvoicePdf(int id) {
            var ss = GenerateSaleInvoice(_context.salesInvoices.Where(i => i.Id.Equals(id)).Include(i=>i.Products).Include(i=> i.Customer).FirstOrDefault());
            return Json(ss);
        }

    }
}
