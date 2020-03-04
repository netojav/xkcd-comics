using System;
using Xunit;
using xkcd_comics.Controllers;
using xkcd_comics.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace xkcd_comics.tests
{
    public class UnitTestsHomeController
    {
        [Fact]
        public async Task LastestComicShouldHaveOnlyPrevNum()
        {
            var controller = new HomeController();
            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ComicModel>(viewResult.ViewData.Model);

            Assert.True(model.num > model.prevNum);
            Assert.Equal(0, model.nextNum);
        }



        [Fact]
        public async Task ComicNumNotExists()
        {
            var notFoundNum = 404;
            var controller = new HomeController();

            var result = await controller.Comic(notFoundNum);
            var notFoundResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("404", notFoundResult.ViewName);
        }

        [Fact]
        public async Task IfPrevNumNotExistsShouldSkipToPrevThatExists()
        {
            var num =  405;
            var controller = new HomeController();

            var result = await controller.Comic(num);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ComicModel>(viewResult.ViewData.Model);

            Assert.Equal(403, model.prevNum);
            
        }

        [Fact]
        public async Task IfNextNumNotExistsShouldSkipToNextThatExists()
        {
            var num =  403;
            var controller = new HomeController();

            var result = await controller.Comic(num);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ComicModel>(viewResult.ViewData.Model);

            Assert.Equal(405, model.nextNum);
            
        }

    }
}
