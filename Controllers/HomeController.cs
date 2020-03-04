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

        private async Task<ComicModel> GetComic(int? num = null)
        {
            var comic = new ComicModel();
            var comicNumPath = num.HasValue ? $"{num.Value}/" : string.Empty;
            using (_httpClient)
            {
                using (var response = await _httpClient.GetAsync($"{comicNumPath}info.0.json"))
                {
                    var json = await response.Content.ReadAsStringAsync();
                    comic = JsonConvert.DeserializeObject<ComicModel>(json);
                    comic.prevNum = comic.num - 1;
                    comic.nextNum = num.HasValue ? comic.num + 1 : 0;
                }
            }

            return comic;
        }

        public async Task<IActionResult> Index()
        {
            var lastComic = await GetComic();

            return View(lastComic);

        }


        public async Task<IActionResult> Comic(int num)
        {
            var comicByNum = await GetComic(num);

            return View(comicByNum);
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
