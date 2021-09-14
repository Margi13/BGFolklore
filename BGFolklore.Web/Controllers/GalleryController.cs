using BGFolklore.Models.Gallery.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BGFolklore.Web.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly IGalleryService galleryService;

        public GalleryController(ILogger<GalleryController> logger,
            IWebHostEnvironment webHostEnvironment,
            IGalleryService galleryService) : base(logger, webHostEnvironment)
        {
            this.galleryService = galleryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Images()
        {

            string jsonString = LoadJson();
            IList<AreaImagesViewModel> viewModelList = galleryService.GetImagesFromJson(jsonString);
            return View(viewModelList);
        }
        public IActionResult Videos()
        {
            string jsonString = LoadJson();
            IList<AreaVideosViewModel> viewModelList = galleryService.GetVideosFromJson(jsonString);
            return View(viewModelList);
        }

        //Common, ErrorHandling
        public static string LoadJson()
        {
            string jsonString;
            using (StreamReader r = new StreamReader("./areas.json", Encoding.UTF8))
            {
                jsonString = r.ReadToEnd();
            }
            return jsonString;
        }

    }

}
