using Amazon.Runtime;
using Amazon.Textract;
using Google.Apis.Storage.v1.Data;
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
using NuGet.Packaging.Signing;
using Smart_Invoice.Areas.Identity.Pages.Account;
using Smart_Invoice.Data;
using Smart_Invoice.Models;
using Smart_Invoice.Models.Invoices;
using Smart_Invoice.Models.Products;
using Smart_Invoice.Models.Stock;
using Smart_Invoice.Models.Warehouse;
using Smart_Invoice.Utility;
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
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly AWSCredentials _awsCredentails;
        public InvoicesController(ApplicationDbContext context, ILogger<LoginModel> logger, IOptions<OpenAiSettings> settings)
        {
            _context = context;
            _logger = logger;
            _OpenAi = settings.Value.apiKey;
            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
            };
            var chain = new Amazon.Runtime.CredentialManagement.CredentialProfileStoreChain();
            if(!chain.TryGetAWSCredentials("default",out _awsCredentails))
            {
                throw new Exception("aws credentails ");
            }
        }

        // GET: Accountant/Invoices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoices.Include(i => i.CompanyID);
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
                invoice = await _context.ProductInvoices.Include(d => d.Items).FirstOrDefaultAsync(m => m.Id.Equals(id));
                invoiceViewModel.ProductInvoice = (Product_Invoice)invoice;
            }
            else if (invoice is UtilityInvoice utility)
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
                    var document = await VisionExtract(imageProto);
                    var respone = await CallOpenAi(document);
                    ViewBag.response = respone;

                    /*string response;
                    using (StreamReader reader = new StreamReader("./Test Files/Test3.json"))
                    {
                        var responseOBj = JsonConvert.DeserializeObject<Invoice>(reader.ReadToEnd());
                        response = JsonConvert.SerializeObject(responseOBj);
                    }*/
                    TempData["model"] = respone;
                    HttpContext.Session.SetString("model", respone);

                    HttpContext.Session.SetString("image", Convert.ToBase64String(imageBytes).ToString());
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

            try
            {
                if (TempData["viewModel"] == null)
                {
                    string Sresponse = HttpContext.Session.GetString("model");

                    int jsonStartIndex = Sresponse.IndexOf("{"); // Find the index of the opening brace of the JSON object
                    string jsonOnly = Sresponse.Substring(jsonStartIndex); // Extract the JSON part
                    Sresponse = jsonOnly;
                    var parsedJson = JObject.Parse(Sresponse.ToLower());
                    JObject filteredJson = new JObject(
                        new JProperty("Company", parsedJson["company"]),
                        new JProperty("invoice_number", parsedJson["invoice_number"]),
                        new JProperty("Invoice_Date", parsedJson["invoice_date"]),
                        new JProperty("Subtotal", parsedJson["subtotal"]),
                        new JProperty("Tax", parsedJson["tax"]),
                        new JProperty("Total", parsedJson["total"]),
                        new JProperty("Items", parsedJson["items"]),
                        new JProperty("Currency", parsedJson["currency"])
                    );
                    /* using (StreamReader reader = new StreamReader("./Test Files/Test3.json"))
                     {
                         Sresponse = reader.ReadToEnd();
                     }*/
                    InvoiceViewModel viewModel = new InvoiceViewModel();

                    if (Sresponse.ToLower().Contains("items"))
                    {
                        viewModel.ProductInvoice = JsonConvert.DeserializeObject<Product_Invoice>(filteredJson.ToString(), _serializerSettings);
                        ViewData["Companies"] = new SelectList(SearchRelatedCompanies(viewModel.ProductInvoice.Company.Company_Name), "Company_Name", "Company_Name");
                        var invoiceID = _context.Invoices.FirstOrDefault(I => I.Invoice_Number.Equals(viewModel.ProductInvoice.Invoice_Number));
                        if (invoiceID != null)
                        {
                            var Error = new Toastr(SD.ToastError, "Invoice Was Already been Captured!");
                            TempData["Toastr"] = JsonConvert.SerializeObject(Error);
                            return RedirectToAction(nameof(Index));
                        }
                        List<ProductMatches> result = await CheckItems(viewModel.ProductInvoice);
                        viewModel.ProductMatches = result;
                        List<SelectListItem> optionsList = new List<SelectListItem>();
                        List<Product> missingProducts = new List<Product>();
                        foreach (var item in result)
                        {
                            if (item.Invoiceproduct != null)
                            {
                                continue;
                            }
                            else
                            {
                                SelectListItem listItem = new SelectListItem
                                {
                                    Text = item.Bestmatch,
                                    Value = item.Bestmatch,
                                };
                                optionsList.Add(listItem);
                                HttpContext.Session.SetString("ProductInvoice", JsonConvert.SerializeObject(viewModel, _serializerSettings).ToString());
                                InvoiceItem Item = viewModel.ProductInvoice.Items.Where(p => p.Name.ToLower().Contains(item.Product.ToLower())).FirstOrDefault();
                                if (Item == null)
                                {

                                }
                                else
                                {
                                    Product product1 = new Product();
                                    product1.Name = Item.Name;
                                    product1.CostPrice = Item.UnitPrice;
                                    product1.Price = Item.UnitPrice + (Item.UnitPrice * 0.16);
                                    product1.CreatedDate = DateTime.Now;
                                    product1.UpdatedDate = DateTime.Now;
                                    missingProducts.Add(product1);
                                }
                                /* HttpContext.Session.SetString("NextView", "Edit");


                                 return RedirectToAction("Create", "Products");*/
                            }

                        }

                        HttpContext.Session.SetString("Product", JsonConvert.SerializeObject(missingProducts.DistinctBy(p => p.Name)));
                        HttpContext.Session.SetString("ViewModel", JsonConvert.SerializeObject(viewModel));
                        ViewData["select"] = new SelectList(optionsList, "Value", "Text");

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

                    string base64Image = HttpContext.Session.GetString("image");
                    ViewBag.Base64Image = base64Image.ToString();

                    return View(viewModel);
                }
                else
                {
                    InvoiceViewModel invoiceView = JsonConvert.DeserializeObject<InvoiceViewModel>(TempData["viewModel"].ToString());


                    string base64Image = HttpContext.Session.GetString("image");
                    ViewBag.Base64Image = base64Image;

                    return View(invoiceView);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
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
                HttpContext.Session.Remove("model");
                HttpContext.Session.Remove("image");
                HttpContext.Session.Remove("Product");
                HttpContext.Session.Remove("ViewModel");
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
                            if (property.Name.Contains("Total") || property.Name.Contains("Tax")
                                || property.Name.Contains("Subtotal"))
                            {
                                property.SetValue(invoice, Double.Parse(value));
                            }
                            else
                            {
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
                    if (rawInvoice.Keys.Any(k => k.StartsWith("Product")) || rawInvoice.Keys.Any(k => k.StartsWith("UnitPrice")))
                    {
                        Product_Invoice invoice = new Product_Invoice();
                        List<InvoiceItem> items = new List<InvoiceItem>();
                        List<Inventory> inventories = new List<Inventory>();
                        var i = 0;
                        foreach (var item in rawInvoice.Keys)
                        {
                            var property = typeof(Product_Invoice).GetProperty(item);
                            if (property != null)
                            {
                                var value = rawInvoice[item].ToString();
                                if (property.Name.Contains("Tax") || property.Name.Contains("Total")
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
                                Inventory inventory = new Inventory();

                                InvoiceItem itemInvoice = new InvoiceItem();
                                itemInvoice.Name = rawInvoice["Product_" + i];
                                itemInvoice.Unit = rawInvoice["Unit_" + i];
                                itemInvoice.UnitPrice = Double.Parse(rawInvoice["UnitPrice_" + i]);
                                itemInvoice.Quantity = Int32.Parse(rawInvoice["Quantity_" + i]);
                                itemInvoice.Total = Double.Parse(rawInvoice["Total_" + i]);
                                itemInvoice.productId = _context.Products.Where(p => p.Name.Contains(itemInvoice.Name)).Select(p => p.ProductId).FirstOrDefault();
                                if (itemInvoice.productId != null)
                                {
                                     inventory = _context.Inventories.Where(i => i.ProductId.Equals(itemInvoice.productId)).FirstOrDefault();
                                    if (inventory != null)
                                    {
                                        inventory.ProductCount = (int)itemInvoice.Quantity;
                                        inventory.PurchaseDate = DateTime.UtcNow;
                                        inventory.LastUpdated = DateTime.UtcNow;
                                        inventory.SKU = "as";
                                        inventory.ProductId = (int)itemInvoice.productId;
                                        inventory.SellingPrice = itemInvoice.Total + (itemInvoice.Total + 0.16);
                                        Warehouse warehouse = _context.Warehouses.Find(1);
                                        warehouse.AvailableSpace = warehouse.AvailableSpace - (int)itemInvoice.Quantity;
                                        warehouse.OccupancyRate = warehouse.Capacity / warehouse.AvailableSpace;

                                    }
                                    else
                                    {
                                        inventory = new Inventory();
                                        inventory.ProductCount = inventory.ProductCount + (int)itemInvoice.Quantity;
                                        inventory.PurchaseDate = DateTime.UtcNow;
                                        inventory.LastUpdated = DateTime.UtcNow;
                                        inventory.ProductId = (int)itemInvoice.productId;
                                        inventory.SKU = "as";
                                        inventory.SellingPrice = itemInvoice.Total + (itemInvoice.Total + 0.16);
                                        Warehouse warehouse = _context.Warehouses.Find(1);
                                        warehouse.AvailableSpace = warehouse.AvailableSpace - (int)itemInvoice.Quantity;
                                        warehouse.OccupancyRate = warehouse.Capacity / warehouse.AvailableSpace;
                                    }
                                }
                                i++;
                                items.Add(itemInvoice);
                                inventories.Add(inventory);
                            }
                        }

                        var company = _context.Companies.Where(c => c.Company_Name.
                                Contains(rawInvoice["Company.Company_Name"])).FirstOrDefault();

                        invoice.Company = company;
                        invoice.Items = items.ToArray();
                        invoice.Invoice_Id = Guid.NewGuid().ToString();
                        invoice.Invoice_Type = SD.InvoiceProduct;
                        invoice.CompanyID = company;
                        _context.ProductInvoices.Add(invoice);
                        _context.Inventories.AddRange(inventories);
                        HttpContext.Session.Remove("model");
                        HttpContext.Session.Remove("image");
                        HttpContext.Session.Remove("Product");
                        HttpContext.Session.Remove("ViewModel");
                        await _context.SaveChangesAsync();

                    }
                }


                _logger.LogInformation(User.Identity.Name + "Has Submited a new Document ");
                HttpContext.Session.Remove("Product");
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
                    return false;
                }
            }
            else
            {
                // Return a bad request response
                return false;
            }
        }
        public async Task<Google.Cloud.DocumentAI.V1.Document> ExtractInvoice(IFormFile file)
        {
            const string projectId = "document-ea-369818";
            const string locationId = "eu";
            const string processorId = "578778ce00c7a18";
            const string mimeType = "image/jpg";
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
                var table = response.Document.Pages[0].Tables[0];




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

            var prompt = SD.Productprompt_v2 + rawdocument;
            var apiKey = _OpenAi;
            var model = "text-davinci-003";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var data = new { prompt = prompt, model = model, max_tokens = 1500 };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.openai.com/v1/completions", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(responseString);
            var choices = jsonObject["choices"].First;
            var text = choices["text"].ToString();

            return text;

        }
        public async Task<string> GPTCValidateItems(List<string> potentialMatches, List<string> products)
        {
            var productsString = string.Join(", ", products);
            var potentialMatchesString = string.Join(", ", potentialMatches);
            var prompt = SD.CheckForItemsPrompt + " Invoice_product[ " + productsString +
                "] and database_products: [" + potentialMatchesString + "] " + SD.CheckForItemsPromptC;
            var apiKey = _OpenAi;
            var model = "text-davinci-003";

            /* building the equest */
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var data = new { prompt = prompt, model = model, max_tokens = 1500 };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.openai.com/v1/completions", content);

            /* response */
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(responseString);
            var choices = jsonObject["choices"].First;
            var text = choices["text"].ToString();
            return text;
        }
        /* Extract the Document/Image Text using OCR  */
        public async Task<string> VisionExtract(Image image)
        {
            
            ImageAnnotatorClient client =  ImageAnnotatorClient.Create();
            //TODO: use text annotator and search for the block type table  
            //Image image = Image.FromFile("./Modified Images/IMG_3205.jpg");
            AnnotateImageRequest request = new AnnotateImageRequest();
            
            request.Image = image;
            request.Features.Add(new Feature
            {
                Type = Feature.Types.Type.DocumentTextDetection,
                
            });
            AnnotateImageResponse response = client.Annotate(request);
            
           
            var structuredText = RestructureTextWithVertices(response.FullTextAnnotation.Pages[0].Blocks.ToList());
            return response.FullTextAnnotation.Text;
           
        }

        /* This Method will take all the block in the response and reformat the text accourding to the x-y coordenates which will help the 
            GPT model to construct the product table and know excatly the product names and their prices so no conflict will appear */
        public static string RestructureTextWithVertices(List<Google.Cloud.Vision.V1.Block> extractedText)
        {
            
            List<string> document = new List<string>();
            List<Word> allwords = new List<Word>();
            // Sort the extracted text elements based on both X and Y coordinates
            var sortedText = extractedText.OrderBy(elem=> elem.BoundingBox.Vertices[3].X).ToList();
            List<string> test1 = new List<string>();
            List<int> yCoordinates = new List<int>();
            foreach (var block in sortedText)
            {
                List<string> paragraphs = new List<string>();

                foreach (var paragraph in block.Paragraphs)
                {

                    allwords.AddRange(paragraph.Words);
                    foreach (var item in paragraph.Words)
                    {
                        yCoordinates.Add(item.BoundingBox.Vertices[0].X);
                        test1.Add(string.Join("", item.Symbols.Select(e => e.Text)) );
                    }

                }

            }
            var spacing = GetAverageSpacing(yCoordinates).Result;

            Word previousWord = allwords[0];
            List<Word> currentLine = new List<Word>();
            List<string> lines = new List<string>();
            currentLine.Add(previousWord);
            for (int i = 1; i < allwords.Count; i++)
            {
                Word currentWord = allwords[i];
                double currentAvg = 0.0;
                double previousAvg = 0.0;
                for (int j = 0; j < 4; j++)
                {
                    currentAvg += currentWord.BoundingBox.Vertices[j].X;
                    previousAvg += previousWord.BoundingBox.Vertices[j].X;
                }
                currentAvg = currentAvg / 4;
                previousAvg = previousAvg / 4;
                if (Math.Abs(currentAvg - previousAvg) <= 15)
                {
                    currentLine.Add(currentWord);
                }
                else
                {
                    lines.Add(ConcatenateWordsInLine(currentLine));
                    previousWord = currentWord;
                    currentLine = new List<Word> { currentWord };
                }
            }
            lines.Add(ConcatenateWordsInLine(currentLine));
            string reconstructedText = string.Join("\n", lines);
           


            return reconstructedText;

        }
        public static string ConcatenateWordsInLine(List<Word> Words)
        {
            List<string> line = new List<string>();
            foreach (var word in Words)
            {
                line.Add(string.Join("", word.Symbols.Select(elm => elm.Text)));
            }
            return string.Join(" ", line);
        }


        public Task<string> CheckInvoicePrices(InvoiceViewModel viewModel)
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

        private static async Task<double> GetAverageSpacing(List<int> yCoordinates)
        {
            // Convert the string to an array of integers
            yCoordinates.Sort();
            List<double> lineSpacings = new List<double>(); // List to store average spacings for each line

            List<int> currentLine = new List<int>();
            int previousY = yCoordinates[0];

            // Iterate over the Y coordinates and calculate the average spacing for each line
            foreach (int y in yCoordinates)
            {
                if (Math.Abs(y - previousY) > 10)
                {
                    double averageSpacing = CalculateAverageSpacing(currentLine);
                    lineSpacings.Add(averageSpacing);
                    currentLine.Clear();
                }

                currentLine.Add(y);
                previousY = y;
            }

            // Calculate and store the average spacing for the last line
            double lastLineAverageSpacing = CalculateAverageSpacing(currentLine);
            lineSpacings.Add(lastLineAverageSpacing);
            double averageSpacing2 = 0.0;
            foreach (double line in lineSpacings)
            {
                averageSpacing2 += line;
            }
            averageSpacing2 = averageSpacing2 / lineSpacings.Count;

            return averageSpacing2;
        }
        static double CalculateAverageSpacing(List<int> line)
        {
            double totalSpacing = 0;
            for (int i = 1; i < line.Count; i++)
            {
                int spacing = Math.Abs(line[i] - line[i - 1]);
                totalSpacing += spacing;
            }

            return totalSpacing / (line.Count - 1);
        }

        public async Task<Boolean> CheckCompany(string companyName)
        {
            if (!string.IsNullOrEmpty(companyName))
            {
                var company = await _context.Companies.Where(c => EF.Functions.Like(c.Company_Name_English, "%" + companyName.ToLower() + "%")).FirstOrDefaultAsync();
                if (company != null)
                {
                    return true;
                }

            }

            return false;

        }

        public async Task<List<ProductMatches>> CheckItems(Smart_Invoice.Models.Invoices.Product_Invoice Invoices)
        {
            List<string> InValid = new List<string>();
            List<Product> Valid = new List<Product>();
            if (Invoices != null && Invoices.Items != null)
            {
                List<string> items = Invoices.Items.Select(n => n.Name).ToList();
                /*int batchSize = 10;
                var batches = Enumerable.Range(0, (items.Count + batchSize - 1) / batchSize)
                        .Select(i => items.Skip(i * batchSize).Take(batchSize));
*/

                HashSet<string> uniqueMatches = new HashSet<string>();
                HashSet<KeyValuePair<string, string>> keyValuePairs = new HashSet<KeyValuePair<string, string>>();
                List<string> potentialMatchesTrial = new List<string>();
                List<string> toBeRemovedFromInValid = new List<string>();

                foreach (string nonExistingProductName in items)
                {
                    //problem if it found a match for one product and not the others what should it do 
                    //TODO: make a new list and if there was a match in the search remove it from the original list and add it to the found list 
                    // so the non exisiting products that wasn't found can be researched 
                    try
                    {
                        var flag = uniqueMatches.Count;
                        keyValuePairs.Add(new KeyValuePair<string, string>(nonExistingProductName, string.Join(",", SearchCritira(nonExistingProductName))));
                        keyValuePairs.Add(new KeyValuePair<string, string>(nonExistingProductName, string.Join(",", WordSearchCritira(nonExistingProductName))));
                        uniqueMatches.AddRange(SearchCritira(nonExistingProductName));
                        uniqueMatches.AddRange(WordSearchCritira(nonExistingProductName));
                        if (uniqueMatches.Count != flag)
                        {
                            toBeRemovedFromInValid.Add(nonExistingProductName);
                        }
                    }
                    catch (Exception ex)
                    {

                    }



                }
                InValid.AddRange(keyValuePairs.Where(p => string.IsNullOrEmpty(p.Value)).Select(p => p.Key));
                keyValuePairs.RemoveWhere(pair => string.IsNullOrEmpty(pair.Value));

                //TODO : if it found everything in the items 
                if (uniqueMatches.Count >= items.Count)
                {
                    List<ProductMatches> productMatches = new List<ProductMatches>();
                    int flag = 0;
                    foreach (var item in keyValuePairs)
                    {
                        ProductMatches productTemp = new ProductMatches();

                        productTemp.Bestmatch = item.Value;
                        productTemp.Invoiceproduct = _context.Products.Where(p => p.Name.Equals(item.Value)).FirstOrDefault();
                        productTemp.Product = item.Key;
                        flag++;
                        productMatches.Add(productTemp);
                    }
                    return productMatches;
                }
                if (uniqueMatches.Count > 0)
                {

                    List<ProductMatches> productMatches = new List<ProductMatches>();
                    foreach (var item in keyValuePairs)
                    {
                        ProductMatches matches1 = new ProductMatches();
                        matches1.Product = item.Key;
                        matches1.Bestmatch = item.Value;
                        matches1.Invoiceproduct = _context.Products.Where(p => p.Name.Equals(item.Value)).FirstOrDefault();
                        productMatches.Add(matches1);
                    }
                    /* found some products not all */
                    List<string> matches = new List<string>();

                    foreach (string nonExistingProductName in InValid)
                    {
                        if (toBeRemovedFromInValid.Contains(nonExistingProductName))
                        {
                            continue;
                        }
                        matches = SearchByThreshold(nonExistingProductName.Length, nonExistingProductName, potentialMatchesTrial);
                        if (matches.Count == 0)
                        {
                            matches = SearchByThreshold(45, nonExistingProductName, potentialMatchesTrial);
                        }




                    }
                    /* TODO: Call GPT to return each product and its possible name if not equal should return null */
                    if (matches.Count != 0)
                    {
                        var result = await GPTCValidateItems(matches.ToList(), InValid.ToList());

                        int jsonStartIndex = result.IndexOf("{"); // Find the index of the opening brace of the JSON object
                        string jsonOnly = result.Substring(jsonStartIndex); // Extract the JSON part
                        result = jsonOnly;
                        try
                        {
                            JObject parsedJson = JObject.Parse(result);
                            JObject filteredjson = new JObject(
                                new JProperty("ProductMatches", parsedJson["ProductMatches"]));
                            productMatches.AddRange(JsonConvert.DeserializeObject<ListProductMatches>(filteredjson.ToString()).ProductMatches);

                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    return (productMatches);


                }
                else
                {
                    List<ProductMatches> productMatches = new List<ProductMatches>();

                    /* Couldn't find any product form the search critira search all the database should be an extreem method */
                    foreach (string nonExistingProductName in items)
                    {
                        List<string> matches;
                        matches = SearchByThreshold(nonExistingProductName.Length, nonExistingProductName, potentialMatchesTrial);
                        if (matches.Count == 0)
                        {
                            matches = SearchByThreshold((int)Math.Floor(nonExistingProductName.Length * 1.5), nonExistingProductName, potentialMatchesTrial); if (matches.Count != 0)
                            {

                            }
                        }
                        else
                        {
                            //Should not remove but replace and then remove 

                        }

                        foreach (var item in matches.ToList())
                        {
                            ProductMatches product = new ProductMatches();
                            product.Product = nonExistingProductName;
                            product.Bestmatch = item;
                            productMatches.Add(product);
                        }
                    }

                    return (productMatches);

                }
            }
            return null;

        }
        /* calculate the levenshtein Distance for two strings */
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
        /* this method will search the database using the three SQL methods contains(),startsWith(),endsWith() to ensure that the database contains the product
         * so we don't have to search the entire databse nut only a subset the smaller it gets the better 
         */
        public List<string> SearchCritira(string nonExistingProductName)
        {

            var matches = _context.Products.Where(p => p.Name.ToLower().Contains(nonExistingProductName.ToLower()) ||
            p.Name.ToLower().StartsWith(nonExistingProductName) ||
            p.Name.ToLower().EndsWith(nonExistingProductName)).Select(p => p.Name).ToList();

            return matches;


        }
        /* This method will search the database for the invoice products by splitting the string to two words and searching to 
            Improve the accuracy of retreiving the product from the databse */
        public List<string> WordSearchCritira(string nonExistingProductName)
        {
            if (nonExistingProductName == null || nonExistingProductName.Split(" ").Length == 1)
            {
                /* the product is one word  */
                return new List<string>();
            }
            else
            {
                var words = nonExistingProductName.ToLower().Split(" ");
                var searchWord = words[0] + " " + words[1];
                var result = _context.Products.Where(p => p.Name.ToLower().Contains(searchWord)).Select(p => p.Name).ToList();
                var result2 = _context.Products
           .Where(p => EF.Functions.Like(p.Name, "%" + searchWord + "%"))
           .Select(p => p.Name)
           .ToList();
                return result;
            }
        }

        /* this function will search the entire database and calculate the Levenshtein Distance and return the matches using the threshold */
        public List<string> SearchByThreshold(int threshold, string nonExistingProductName, List<string> potentialMatchesTrial)
        {
            // search in the potentail mathces 
            var matchesTrial = potentialMatchesTrial.Where(p => CalculateLevenshteinDistance(nonExistingProductName.ToLower(), p.ToLower()) <= threshold).ToList();

            if (potentialMatchesTrial.Count == 0)
            {
                var matches = _context.Products
                                  .AsEnumerable()
                                  .Where(p => CalculateLevenshteinDistance(nonExistingProductName, p.Name) <= threshold).Select(p => p.Name)
                                  .ToList();
                return matches;
            }

            return matchesTrial;
        }
        /* This method is being called from AJAX which will help seed the Product Model befor opening the popup */
        public async Task<IActionResult> CreateProduct(string product, InvoiceViewModel viewModel)
        {


            InvoiceItem Item = viewModel.ProductInvoice.Items.Where(p => p.Name.Equals(product)).FirstOrDefault();
            Product product1 = new Product();
            product1.Name = Item.Name;
            product1.CostPrice = Item.UnitPrice;
            product1.Price = Item.UnitPrice * 0.16;
            product1.CreatedDate = DateTime.Now;
            product1.UpdatedDate = DateTime.Now;

            TempData["Product"] = JsonConvert.SerializeObject(product1);
            return RedirectToAction("Index");


        }
        /* This method will search for companies that may be an close to the company being extracted */
        public List<Company> SearchRelatedCompanies(string product)
        {
            if (product.Split(" ").Length > 1)
            {
                var words = product.Split(" ");
                var result = _context.Companies.Where(p => p.Company_Name.ToLower().Contains(words[0] + " " + words[1])).ToList();
                if (result.Count == 0)
                {
                    result = _context.Companies.ToList();
                }
                return result;
            }
            else if (product.Split(" ").Length == 1)
            {
                var result = _context.Companies.Where(p => p.Company_Name.ToLower().Contains(product)).ToList();
                if (result.Count == 0)
                {
                    result = _context.Companies.ToList();
                }
                return result;
            }
            return null;
        }

        [HttpGet]
        public IActionResult GetBillItems()
        {
            string invoice = HttpContext.Session.GetString("ProductInvoice");
            if (invoice != null)
            {
                InvoiceViewModel viewModel = JsonConvert.DeserializeObject<InvoiceViewModel>(invoice);
                if (viewModel != null && viewModel.ProductInvoice != null && viewModel.ProductInvoice.Items != null)
                {
                    List<InvoiceItem> items = (List<InvoiceItem>)viewModel.ProductInvoice.Items;
                    return Json(new { data = items.ToArray() });
                }
            }
            return Json(new { data = new List<InvoiceItem>() }); // Return an empty list if there are no items

        }

        #endregion
    }
}
