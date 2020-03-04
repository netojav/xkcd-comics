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
using System.Net;

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
            ComicModel comic;
            var comicNumPath = num.HasValue ? $"{num.Value}/" : string.Empty;

            using (var response = await _httpClient.GetAsync($"{comicNumPath}info.0.json"))
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                var json = await response.Content.ReadAsStringAsync();
                comic = JsonConvert.DeserializeObject<ComicModel>(json);

            }


            return comic;
        }

        public async Task<IActionResult> Index()
        {
            var lastComic = await GetComic();
            lastComic.prevNum = lastComic.num - 1;
            return View(lastComic);

        }


        public async Task<IActionResult> Comic(int num)
        {
            var lastComic = await GetComic();
            var lastComicId = lastComic.num;

            var comicByNum = await GetComic(num);
            comicByNum.prevNum = comicByNum.num - 1;

            comicByNum.nextNum = comicByNum.num < lastComicId ? comicByNum.num + 1 : 0;


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
