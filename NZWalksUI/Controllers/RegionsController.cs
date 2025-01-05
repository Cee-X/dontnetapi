using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using NZWalksUI.Models.DTO;

namespace NZWalksUI.Controllers
{
    public class RegionsController : Controller
    {
        // GET: RegionsController
        private readonly IHttpClientFactory _httpClientFactory;
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<RegionDTO> regions = new List<RegionDTO>(); 
            try
            {
                //Get all regions from the api 
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage =  await client.GetAsync("https://localhost:7279/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();;
                regions.AddRange(response);
              
            }
            catch (Exception ex)
            {
                
                ViewBag.Response = ex.Message;
            }
           
            return View(regions);
        }
        
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        //Create a new region

        [HttpPost]

        public async Task<ActionResult> Add(RegionDTO region)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7279/api/regions"),
                    Content = new StringContent(JsonSerializer.Serialize(region), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage =  await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();
                
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
                if(response != null)
                {
                    return RedirectToAction("Index", "Regions");
                }
                
            }
            catch (Exception ex)
            {
                
                throw;
            }
            return View();
        }

        //GET : Edit Single Region
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var httpResponseMessage = await client.GetAsync($"https://localhost:7279/api/regions/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
                
            if(response != null)
                {
                    return View(response);
                }
            return View();

        }

        //Upadte Region
        [HttpPut]

        public async Task<ActionResult> Update(RegionDTO region)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpRequestMessage= new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7279/api/regions/{region.Id}"),
                    Content = new StringContent(JsonSerializer.Serialize(region), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
                if(response != null)
                {
                    return RedirectToAction("Index", "Regions");
                }


            }
            catch (System.Exception)
            {
                
                throw;
            }
            

        }
    }
}
