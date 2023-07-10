using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Smart_Invoice.Data;
using Smart_Invoice.Models;
using Smart_Invoice.Models.Invoices;
using Smart_Invoice.Models.Products;
using System.Text;
using Product = Smart_Invoice.Models.Products.Product;

namespace Smart_Invoice.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accountant/Products
        public async Task<IActionResult> Index()
        {
              return _context.Products != null ? 
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }

        // GET: Accountant/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Accountant/Products/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Product") != null )
            {
                Product product = JsonConvert.DeserializeObject<Product>(HttpContext.Session.GetString("Product"));
                HttpContext.Session.Remove("Product");
               
                
                    return View(product);

                
            }
            return View();
        }

        // POST: Accountant/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,CostPrice,CategoryId,SKU,ImageUrl,CreatedDate,UpdatedDate,IsAvailable,IsActive")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                var nextView = HttpContext.Session.GetString("NextView");
                if (nextView != null)
                {
                    try
                    {
                        Product Temproduct = new Product();
                        Temproduct = JsonConvert.DeserializeObject<Product>(HttpContext.Session.GetString("Product"));

                        HttpContext.Session.Remove("NextView");
                        InvoiceViewModel invoiceView = JsonConvert.DeserializeObject<InvoiceViewModel>(TempData["viewModel"].ToString());
                        if (invoiceView != null)
                        {
                            ProductMatches newproduct = invoiceView.ProductMatches.Where(p => p.Product.Equals(Temproduct.Name)).FirstOrDefault();
                            newproduct.Invoiceproduct = product;
                            newproduct.Bestmatch = product.Name;
                            TempData["viewModel"] = JsonConvert.SerializeObject(invoiceView);

                        }
                        HttpContext.Session.Remove("Product");
                        return RedirectToAction("Edit", "Invoices");
                    }
                    catch (Exception e)
                    {

                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Accountant/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Accountant/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,CostPrice,CategoryId,SKU,ImageUrl,CreatedDate,UpdatedDate,IsAvailable,IsActive")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Accountant/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Accountant/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> AddProductPopup(string itemId)
        {
            // Retrieve the necessary data for the popup, such as existing products, etc.
            var jsonResult = HttpContext.Session.GetString("Product");
            List<Product> product = JsonConvert.DeserializeObject<List<Product>>(jsonResult);
            try
            {
                ViewData["Category"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            }
            catch (Exception ex) { 
            }
            return PartialView("_CreateProduct",product.Where(p=> p.Name.Equals(itemId)).FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> SubmitProduct([FromBody] Product product)
        {
            product.IsActive = true;
            product.IsAvailable = true;
            product.SKU = GenerateSKU();
            if (ModelState.IsValid)
            {
                await _context.Products.AddAsync(product);
                _context.SaveChanges();
                return Ok(new { data = product.Name });
            }
            return BadRequest();
        }
        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                return Ok(product); // Return 200 OK with the product as the response body
            }

            return NotFound(); // Return 404 Not Found if the product is not found
        }

        public string GenerateSKU()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sku = "";

            for (var i = 0; i < 8; i++)
            {
                int randomIndex =(int) Math.Floor(new Random().NextDouble() * characters.Length);
                sku += characters[randomIndex];
            }

            return sku;
        }

        [HttpGet]
        public async Task<IActionResult> getCommonProducts(string? company)
        {
            List<Models.Products.Product> products = new List<Product>();
            if (company == null)
            {
                products = _context.Products.ToList<Product>();
                return Ok(new { data = products});
            }
            else {
                
                Company comp = _context.Companies.Where(c => c.Company_Name_English.Equals(company) || c.Company_Name.Equals(company)).FirstOrDefault();
                if (comp != null)
                {
                    var InvoicesIds = _context.Invoices.Where(I => I.Company.CompanyId == comp.CompanyId).Select(i => i.Invoice_Id).ToList();
                    if (InvoicesIds.Count == 0)
                    {
                        var product = _context.Products.Select(p=> p.Name).ToArray();
                        return Ok(new { data = product });
                    }
                    else
                    {
                        var c = (from ii in _context.InvoiceItem
                                 join i in _context.Invoices on ii.ProductInvoiceId equals i.Id
                                 where i.Company.CompanyId == comp.CompanyId
                                 select ii).ToList();
                      
                        return Ok(new { data = c.Select(i => i.Name ).ToArray()});
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet]
        public JsonResult GetAllProducts()
        {
            List<Product> products = _context.Products.ToList();
            return Json(products);
        }
       
        #endregion
    }

}
