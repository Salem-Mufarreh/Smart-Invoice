using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Options;
using Smart_Invoice.Utility;
using System.Text.Json;

namespace Smart_Invoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize(Roles = SD.Role_Admin)]*/
    public class HomeController : Controller
    {
        private readonly string _exchangeAuth;
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory, IOptions<ExchangeRateAPi> exchangeAuth)
        {
            _httpClientFactory = httpClientFactory;
            _exchangeAuth = exchangeAuth.Value.AuthKey;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.apilayer.com/exchangerates_data/convert?to=ILS&from=USD&amount=100");
            request.Headers.Add("apikey", _exchangeAuth);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<JsonElement>(responseStream);
            var rate = responseObject.GetProperty("info").GetProperty("rate").GetDouble();

            ExchangeRateAPi rateOBJ = new ExchangeRateAPi();
            rateOBJ.rate = rate;
            rateOBJ.BaseUSD = "USD";
            rateOBJ.date = DateTime.Now;
            return View(rateOBJ);
        }


        


    }
}
