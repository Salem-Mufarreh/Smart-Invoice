using Google.Cloud.DocumentAI.V1;
using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using Smart_Invoice.Areas.Identity.Pages.Account;
using Smart_Invoice.Data;
using Smart_Invoice.Models.Invoices;
using Smart_Invoice.Models.Products;
using Smart_Invoice.Utility;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Image = Google.Cloud.Vision.V1.Image;
using Product = Smart_Invoice.Models.Products.Product;

namespace Smart_Invoice.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoginModel> _logger;
        private readonly string _OpenAi;
        
        public InvoicesController(ApplicationDbContext context, ILogger<LoginModel> logger, IOptions<OpenAiSettings> settings)
        {
            _context = context;
            _logger = logger;
            _OpenAi = settings.Value.apiKey;
        }

        // GET: Accountant/Invoices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoices.Include(i=> i.CompanyID);
            List<Invoice> invoices = await applicationDbContext.ToListAsync<Invoice>();
            
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
                .Include(i => i.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            InvoiceViewModel invoiceViewModel = new InvoiceViewModel();

            if (invoice is Product_Invoice product_Invoice)
            {
                invoice = await _context.ProductInvoices.Include(d=>d.Items).FirstOrDefaultAsync(m=> m.Id.Equals(id));
                invoiceViewModel.ProductInvoice = (Product_Invoice)invoice;
            }
            else if(invoice is UtilityInvoice utility)
            {
                invoice = await _context.UtilityInvoices.FirstOrDefaultAsync(m => m.Id == id);
                invoiceViewModel.UtilityInvoice = (UtilityInvoice)invoice;
            }
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoiceViewModel);
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
            try
            {
                

                using (var stream = file.OpenReadStream())
                {
                    /*Convert IFromFile to System.Drawing.Image and to Google.Cloud.Vision.Image*/

                    var image = System.Drawing.Image.FromStream(stream);
                    var memoryStream = new MemoryStream();
                    image.Save(memoryStream, ImageFormat.Jpeg);
                    byte[] imageBytes = memoryStream.ToArray();
                    string base64Image = Convert.ToBase64String(imageBytes);

                    var client = ImageAnnotatorClient.Create();
                    var imageContent = ByteString.CopyFrom(imageBytes);
                    var imageProto = new Image()
                    {
                        Content = imageContent,
                    };

                    /* Call API's */
                    //var document = await VisionExtract(imageProto);
                    //var respone = await CallOpenAi(document);
                    //ViewBag.response = respone;

                    string response;
                    using (StreamReader reader = new StreamReader("./Test Files/Mresponse.json"))
                    {
                        var responseOBj = JsonConvert.DeserializeObject<Invoice>(reader.ReadToEnd());
                        response = JsonConvert.SerializeObject(responseOBj);
                    }
                    TempData["model"] = response;
                    HttpContext.Session.Set("image", imageBytes);
                    return RedirectToAction(nameof(Edit));

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View();
           
           
        }

       

        // GET: Accountant/Invoices/Edit/5
        public async Task<IActionResult> Edit()
        {
            /*var response2 = TempData["model"] as string;
            var response = JsonConvert.DeserializeObject<UtilityInvoice>(response2);*/
            try
            {

                string Sresponse;
                using (StreamReader reader = new StreamReader("./Test Files/test2.json"))
                {
                    Sresponse = reader.ReadToEnd();



                }
                InvoiceViewModel viewModel = new InvoiceViewModel();
                
                if (Sresponse.ToLower().Contains("items"))
                {
                    viewModel.ProductInvoice = JsonConvert.DeserializeObject<Product_Invoice>(Sresponse);
                    if (! await CheckCompany(viewModel.ProductInvoice.Company.Company_Name_English)) {//name in english is null 
                        HttpContext.Session.SetString("NextView", "Edit");
                        TempData["viewModel"] = JsonConvert.SerializeObject(viewModel);
                        return RedirectToAction("Create","Companies");
                    }
                    var invoiceID = _context.Invoices.FirstOrDefault(I => I.Invoice_Number.Equals(viewModel.ProductInvoice.Invoice_Number));
                    if (invoiceID != null)
                    {
                        var Error = new Toastr(SD.ToastError, "Invoice Was Already been Captured!");
                        TempData["Toastr"] = JsonConvert.SerializeObject(Error);
                        return RedirectToAction(nameof(Index));
                    }
                    var result = CheckItems(viewModel.ProductInvoice);
                }
                else if (Sresponse.ToLower().Contains("meter_number"))
                {
                    viewModel.UtilityInvoice = JsonConvert.DeserializeObject<UtilityInvoice>(Sresponse);
                    var invoiceID = _context.Invoices.FirstOrDefault(I => I.Invoice_Number.Equals(viewModel.UtilityInvoice.Invoice_Number));
                    if (invoiceID != null)
                    {
                        var Error = new Toastr(SD.ToastError, "Invoice Was Already been Captured!");
                        TempData["Toastr"] = JsonConvert.SerializeObject(Error);
                        return RedirectToAction(nameof(Index));
                    }
                }
                byte[] imageBytes = HttpContext.Session.Get("image");
                string base64Image = Convert.ToBase64String(imageBytes);
                ViewBag.Base64Image = base64Image;

                return View(viewModel);
            
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        // POST: Accountant/Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection rawInvoice)
        {
            
            if (!ModelState.IsValid)
            {
                //TODO: Add Toastr Notifications
                _logger.LogError(ModelState.ToString());
                return RedirectToAction(nameof(Create));
            }
            else
            {
                //TODO: Edit the image so the signature of the user can be displayed on it 
                // or make a new document as and attached the information with it 
                if (rawInvoice.Keys.Contains("Meter_Number"))
                {
                    UtilityInvoice invoice = new UtilityInvoice();
                    //This invoice is Utility Invoice
                    foreach (var item in rawInvoice.Keys)
                    {
                        var property = typeof(UtilityInvoice).GetProperty(item);
                        if (property != null)
                        {
                            var value = rawInvoice[item].ToString();
                            if(property.Name.Contains("Total") || property.Name.Contains("Tax")
                                || property.Name.Contains("Subtotal"))
                            {
                                property.SetValue(invoice, Double.Parse(value));
                            }
                            else { 
                                property.SetValue(invoice, value.Normalize());
                            }
                        }
                    }
                    var company = _context.Companies.Where(c => c.Company_Name_English.
                                Contains(rawInvoice["Company.Company_Name"])).FirstOrDefault();

                    //TODO if company is null Create a new company 
                    invoice.Company = company;
                    invoice.CompanyID = company;
                    invoice.Invoice_Id = Guid.NewGuid().ToString();
                    invoice.Invoice_Type = SD.InvoiceUtility;
                    _context.UtilityInvoices.Add(invoice);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    if (rawInvoice.Keys.Contains("Product 0") || rawInvoice.Keys.Contains("UnitPrice 0"))
                    {
                        Product_Invoice invoice = new Product_Invoice();
                        List<InvoiceItem> items = new List<InvoiceItem>();
                        var i = 0;
                        foreach (var item in rawInvoice.Keys)
                        {
                            var property = typeof(Product_Invoice).GetProperty(item);
                            if (property != null)
                            {
                                var value = rawInvoice[item].ToString();
                                if ( property.Name.Contains("Tax") || property.Name.Contains("Total")
                                    || property.Name.Contains("Subtotal"))
                                {
                                    property.SetValue(invoice, Double.Parse(value));
                                }
                                
                                else
                                {
                                    property.SetValue(invoice, value.Normalize());
                                }
                                
                            }
                            else if (item.StartsWith("Product"))
                            {
                                InvoiceItem itemInvoice = new InvoiceItem();
                                itemInvoice.Name = rawInvoice["Product "+i];
                                itemInvoice.Unit = rawInvoice["Unit " + i];
                                itemInvoice.UnitPrice = Double.Parse(rawInvoice["UnitPrice " + i]);
                                itemInvoice.Quantity = Int32.Parse(rawInvoice["Quantity " + i]);
                                itemInvoice.Total = Double.Parse(rawInvoice["Total "+i]);
                                i++;
                                items.Add(itemInvoice);
                            }
                        }
                        
                        var company = _context.Companies.Where(c => c.Company_Name.
                                Contains(rawInvoice["Company.Company_Name"])).FirstOrDefault();
                        //TODO if company is null Create a new company 
                        invoice.Company = company;
                        invoice.Items = items.ToArray();
                        invoice.Invoice_Id = Guid.NewGuid().ToString();
                        invoice.Invoice_Type = SD.InvoiceProduct;
                        invoice.CompanyID = company;
                        _context.ProductInvoices.Add(invoice);
                        await _context.SaveChangesAsync();
                    }
                }


                _logger.LogInformation(User.Identity.Name + "Has Submited a new Document ");

                return RedirectToAction(nameof(Index));
                //TODO: Process the information and might be a good idea to save the text and the user who changed it with the image 
            }
           
           
        }

        // GET: Accountant/Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Company)
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
        public async Task<Google.Cloud.DocumentAI.V1.Document> ExtractInvoice (IFormFile file)
        {
            const string projectId = "document-ea-369818";
            const string locationId = "eu";
            const string processorId = "578778ce00c7a18";
            const string mimeType = "image/png";
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
            try
            {
                ProcessResponse response = client.ProcessDocument(request);
                var document = response.Document;
                var pagelayout = response.Document.Pages[0].Layout;
                var pageAnchor = pagelayout.TextAnchor;
                var x = pageAnchor.TextSegments[0].StartIndex;
                var y = document.Pages[0].Blocks;

                


                return document;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        

        public async Task<string> CallOpenAi(string rawdocument)
        {
            
            var prompt =  "can you structure an invoice from this data as key-value pair in json : '"+rawdocument+"'";
            var apiKey = _OpenAi;
            var model = "text-davinci-003";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var data = new { prompt = prompt, model = model , max_tokens = 1500};
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.openai.com/v1/completions", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(responseString);
            var choices = jsonObject["choices"].First;
            var text = choices["text"].ToString();

            return text;

        }

        public async Task<string> VisionExtract(Image image)
        {
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            //Image image = Image.FromFile("./Modified Images/IMG_3205.jpg");
            TextAnnotation text = await client.DetectDocumentTextAsync(image);
            
            return text.Text;
        }

        public Task<string> CheckInvoicePrices( InvoiceViewModel viewModel)
        {
            List<string> Valid = new List<string>();
            List<string> inValid = new List<string>();
            var company = _context.Companies.FirstOrDefault(c => c.Company_Name.Equals(viewModel.ProductInvoice.Company.Company_Name));
            if (company != null)
            {
                Valid.Add("Company_Name");
            }
            else
            {
                inValid.Add("Company_Name");
            }
            if (viewModel == null)
            {

            }
            else
            {
                if (viewModel.ProductInvoice != null)
                {
                    Product_Invoice product_ = viewModel.ProductInvoice;
                    var total = product_.Total;
                    if (total > 0)
                    {
                        Valid.Add("Total");
                        var sum = 0.0;
                        foreach (var item in product_.Items)
                        {
                            sum += item.Total;
                        }
                        if (product_.Tax != null || product_.Tax != 0.0)
                        {
                            if (sum + product_.Tax == total)
                            {
                                Valid.Add("Items");
                                Valid.Add("Tax");
                            }
                            else
                            {
                                inValid.Add("Tax");
                                inValid.Add("Items");
                                ModelState.AddModelError("Items", "The sum of item prices does not match the ivoice total.");
                            }
                        }
                        else
                        {
                            if (sum == total)
                            {
                                inValid.Add("Tax");
                                Valid.Add("Items");
                            }
                            else
                            {
                                inValid.Add("Tax");
                                inValid.Add("Items");
                            }
                        }
                    }
                    else
                    {
                        inValid.Add("Total");
                        ModelState.AddModelError("Total", "");
                    }
                }
            }
            ViewBag.Valid = Valid;
            ViewBag.inValid = inValid;
            return null;
        }
       
        public async Task<Boolean> CheckCompany(string companyName)
        {
            if (!string.IsNullOrEmpty(companyName))
            {
                var company =await _context.Companies.Where(c=> c.Company_Name_English.ToLower().Equals(companyName.ToLower())).FirstOrDefaultAsync();
                if (company != null)
                {
                    return true;
                }
               
            }
            
                return false;
            
        }

        public async Task<(List<Product> Valid, List<string> InValid, List<string> potentialMatches)> CheckItems(Smart_Invoice.Models.Invoices.Product_Invoice Invoices) 
        {
            List<string> InValid = new List<string>();
            List<Product> Valid = new List<Product>();
            List<string> potentialMatches = new List<string>();
            if (Invoices != null && Invoices.Items != null )
            {
                List<string> items = Invoices.Items.Select(n => n.Name).ToList();
                int batchSize = 10;
                var batches = Enumerable.Range(0, (items.Count + batchSize - 1) / batchSize)
                        .Select(i => items.Skip(i * batchSize).Take(batchSize));

                foreach (var batch in batches)
                {
                    var products = _context.Products
                          .Where(p => batch.Contains(p.Name))
                          .ToList();
                    try {
                        // Add existing product names to the existingProductNames list
                        Valid.AddRange(products);

                        // Add non-existing product names to the nonExistingProductNames list
                        InValid.AddRange(batch.Except(products.Select(p => p.Name)));
                    }
                    catch (Exception ex)
                    {
                        var e = ex;
                    }
                }
                HashSet<string> uniqueMatches = new HashSet<string>();
                List<string> potentialMatchesTrial = new List<string>();

                foreach (string nonExistingProductName in InValid)
                {
                    //problem if it found a match for one product and not the others what should it do 
                    //TODO: make a new list and if there was a match in the search remove it from the original list and add it to the found list 
                    // so the non exisiting products that wasn't found can be researched 

                    SearchCritira(nonExistingProductName, potentialMatchesTrial);
                    
                    
                }
                uniqueMatches.AddRange(potentialMatchesTrial);
                potentialMatchesTrial = uniqueMatches.ToList();
                if (potentialMatchesTrial.Count > 0)
                {
                    if (potentialMatchesTrial.Count == 1)
                    {
                        return (Valid, InValid, potentialMatches);

                    }
                    else
                    {
                        //TODO: call GPT to find the best match
                    }
                }
                else
                {
                    foreach (string nonExistingProductName in InValid)
                    {


                        var matchesTrial = potentialMatchesTrial.Where(p => CalculateLevenshteinDistance(nonExistingProductName.ToLower(), p.ToLower()) <= 30).ToList();

                        var matches = _context.Products
                                               .AsEnumerable()
                                               .Where(p => CalculateLevenshteinDistance(nonExistingProductName, p.Name) <= 30).Select(p => p.Name)
                                               .ToList();

                        potentialMatches.AddRange(matches);
                    }
                }
                return (Valid, InValid, potentialMatches);
            }
            return (null,null,null);
            
        }
        public int CalculateLevenshteinDistance(string source, string target)
        {
            int[,] dp = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; i++)
                dp[i, 0] = i;

            for (int j = 0; j <= target.Length; j++)
                dp[0, j] = j;

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

                    dp[i, j] = Math.Min(
                        dp[i - 1, j] + 1,
                        Math.Min(
                            dp[i, j - 1] + 1,
                            dp[i - 1, j - 1] + cost
                        )
                    );
                }
            }

            return dp[source.Length, target.Length];
        }
        public void SearchCritira(string nonExistingProductName, List<string> potentialMatchesTrial)
        {
            var x = _context.Products.Where(p => p.Name.ToLower().Contains(nonExistingProductName.ToLower())).Select(p => p.Name).ToList();
            if (x.Count != 0)
            {
                potentialMatchesTrial.AddRange(x);
            }
            else
            {
                var y = _context.Products.Where(p => p.Name.ToLower().StartsWith(nonExistingProductName.ToLower())).Select(p => p.Name).ToList();
                if (y.Count != 0)
                {
                    potentialMatchesTrial.AddRange(y);
                }
                else
                {
                    var z = _context.Products.Where(p => p.Name.ToLower().EndsWith(nonExistingProductName.ToLower())).Select(p => p.Name).ToList();
                    if (z.Count != 0)
                    {
                        potentialMatchesTrial.AddRange(z);
                    }
                }
            }

        }
        

        #endregion
    }
}
