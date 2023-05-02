﻿using Google.Cloud.DocumentAI.V1;
using Google.Cloud.Storage.V1;
using Google.Cloud.Vision.V1;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Smart_Invoice.Areas.Identity.Pages.Account;
using Smart_Invoice.Data;
using Smart_Invoice.Models;
using System.Drawing;
using System.Text;
using static Google.Rpc.Context.AttributeContext.Types;
using Google.Api;
using Image = Google.Cloud.Vision.V1.Image;
using System.Drawing.Imaging;
using Microsoft.Extensions.Options;
using Smart_Invoice.Utility;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var applicationDbContext = _context.UtilityInvoices;
            List<IParsedInvoice> invoices = await applicationDbContext.ToListAsync<IParsedInvoice>();
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
                    /*var document = await VisionExtract(imageProto);
                    var respone = await CallOpenAi(document);
                    ViewBag.response = respone;*/

                    string response;
                    using (StreamReader reader = new StreamReader("./Test Files/Mresponse.json"))
                    {
                        var responseOBj = JsonConvert.DeserializeObject<UtilityInvoice>(reader.ReadToEnd());
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
                IParsedInvoice response;

                using (StreamReader reader = new StreamReader("./Test Files/Mresponse.json"))
                {

                    response = (IParsedInvoice)JsonConvert.DeserializeObject<UtilityInvoice>(reader.ReadToEnd());
            }
            byte[] imageBytes = HttpContext.Session.Get("image");
            string base64Image = Convert.ToBase64String(imageBytes);
            ViewBag.Base64Image = base64Image;
            var company = _context.Companies.Where(x => x.Company_Name.Contains(response.Incoive_Company)).FirstOrDefault();
                if (company != null)
                {
                    ViewBag.Company_License_Registration_Number = company.Company_License_Registration_Number;
                }
                return View(response);
            }
            catch (Exception ex)
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
                            if (property.Name.Contains("Invoice_Amount") || property.Name.Contains("Invoice_VAT"))
                            {
                                property.SetValue(invoice, Double.Parse(value));
                            }
                            else
                            {
                                property.SetValue(invoice, value.Normalize());
                            }
                        }
                    }
                    var company = _context.Companies.Where(c => c.Company_Name.
                    Contains(invoice.Incoive_Company)).FirstOrDefault();
                    //TODO if company is null Create a new company 
                    invoice.Company_Id = company;
                    invoice.Id = Guid.NewGuid().ToString();
                    _context.Add(invoice);
                    await _context.SaveChangesAsync();

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

        /* this function will recive the file from html form and save to temp location,
         * it will read the file from the temp location and transform the pixels to 
         * grayscale color to reduce any noise and focuse more on the fonts.
         * Document Ai already have pre-processing for images*/
        public async Task<Boolean> ApplyFilter(IFormFile file)
        {
            try
            {

                var uploadpath = Path.Combine(Path.GetTempPath(), "Uploaded Files", file.FileName);
                using (var stream = new FileStream(uploadpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                Bitmap bitmap = new Bitmap(uploadpath);
                Color p;
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        p = bitmap.GetPixel(x, y);
                        int a = (int)p.A;
                        int b = (int)p.B;
                        int r = (int)p.R;
                        int g = (int)p.G;

                        int avg = (r + g + b) / 3;
                        bitmap.SetPixel(x, y, Color.FromArgb(avg, avg, avg));

                    }
                }
                var uploadpath2 = Path.Combine(Directory.GetCurrentDirectory(), "Uploaded Files", file.FileName);
                bitmap.Save(uploadpath2);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

            return true;

        }

        public async Task<string> CallOpenAi(string rawdocument)
        {

            var prompt = "can you structure an invoice from this data as key-value pair in json : '" + rawdocument + "'";
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

        public async Task<string> VisionExtract(Image image)
        {
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            //Image image = Image.FromFile("./Modified Images/IMG_3205.jpg");
            TextAnnotation text = await client.DetectDocumentTextAsync(image);

            return text.Text;
        }
        #endregion
    }
}
