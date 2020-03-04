using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using xkcd_comics.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace xkcd_comics.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://xkcd.com");
        }

        public async Task<IActionResult> Index()
        {
            var lastComic = new ComicModel();
            using (_httpClient)
            {
                using (var response = await _httpClient.GetAsync("info.0.json"))
                {
                   var json = await response.Content.ReadAsStringAsync();
                   lastComic = JsonConvert.DeserializeObject<ComicModel>(json);
                }
            }

            return View(lastComic);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
