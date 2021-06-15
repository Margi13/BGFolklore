using BGFolklore.Models.Gallery.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BGFolklore.Web.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IGalleryService galleryService;

        public GalleryController(IWebHostEnvironment webHostEnvironment, IGalleryService galleryService)
        {
            this.webHostEnvironment = webHostEnvironment;
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
