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
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    // Create an instance of System.Drawing.Image from the stream
                    var image = System.Drawing.Image.FromStream(stream);

                    // Create a MemoryStream to write the image data to
                    var memoryStream = new MemoryStream();

                    // Save the image to the MemoryStream in the format you need (e.g. PNG, JPEG)
                    image.Save(memoryStream, ImageFormat.Jpeg);

                    // Get the byte array from the MemoryStream
                    byte[] imageBytes = memoryStream.ToArray();

                    // Convert the byte array to a base64 string if needed
                    string base64Image = Convert.ToBase64String(imageBytes);
                    

                    /* var document = await VisionExtract(image);
                     var respone = await CallOpenAi(document);
                     ViewBag.response = respone;*/
                    string response;
                    using (StreamReader reader = new StreamReader("./Test Files/Mresponse.json"))
                    {
                        var responseOBj = JsonConvert.DeserializeObject<UtilityInvoice>(reader.ReadToEnd());
                        response = JsonConvert.SerializeObject(responseOBj);
                    }
                    

                    //return View(nameof(Edit), (response, base64Image));
                    TempData["model"] = response;
                    HttpContext.Session.Set("image", imageBytes);
                    return RedirectToAction(nameof(Edit));

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
/*            try
            {
                _logger.LogInformation("User is applying filter");
                //await ApplyFilter(file);
                var document = (Google.Cloud.DocumentAI.V1.Document)await ExtractInvoice(file);
                Image image = Image.FromFile( Path.Combine(Directory.GetCurrentDirectory(), "Uploaded Files", file.FileName) );
                var imageWidth = image.Width;
                var imageHeight = image.Height;
                Graphics graphics = Graphics.FromImage(image);
                Pen pen = new Pen(Color.Red, 2);
                foreach (var block in document.Pages[0].Blocks)
                {
                    // If the block contains text, draw a rectangle around it

                    {
                        // Get the location of the block on the page
                        var blockBounds = block.Layout.BoundingPoly.NormalizedVertices;

                        // Calculate the x, y, width, and height of the rectangle
                        float x = blockBounds[0].X * imageWidth;
                        float y = blockBounds[0].Y * imageHeight;
                        float width = (blockBounds[1].X - blockBounds[0].X) * imageWidth;
                        float height = (blockBounds[2].Y - blockBounds[0].Y) * imageHeight;

                        // Draw a rectangle around the text using the calculated x, y, width, and height values
                        graphics.DrawRectangle(pen, x, y, width, height);
                    }
                }
                string outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Modified Images", file.FileName);
                image.Save(outputFilePath);
                graphics.Dispose();
                image.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
*/            return View();
           
        }
        [HttpGet]
        public async Task<IActionResult> DocumentValidate(DocumentValidateModel model)
        {   
            
            return View(model);
        }
       
 

        // GET: Accountant/Invoices/Edit/5
        public async Task<IActionResult> Edit(UtilityInvoice? invoice)
        {
            /*var response2 = TempData["model"] as string;
            var response = JsonConvert.DeserializeObject<UtilityInvoice>(response2);*/
            byte[] imageBytes = HttpContext.Session.Get("image");
            string base64Image = Convert.ToBase64String(imageBytes);
            ViewBag.Base64Image = base64Image;
            return View();
        }

        // POST: Accountant/Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile file)
        {
            try
            {
                var uploadpath = Path.Combine(Directory.GetCurrentDirectory(),"Uploaded Files", file.FileName);
                var stream = new FileStream(uploadpath, FileMode.Create);
                await file.CopyToAsync(stream);

            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
            }
            return View();
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
        #endregion
    }
}
