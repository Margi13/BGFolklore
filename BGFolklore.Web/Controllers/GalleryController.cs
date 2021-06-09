using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using BGFolklore.Web.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;

namespace BGFolklore.Web.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public GalleryController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Images()
        {

            JsonResponse items = LoadJson();
            return View(items);
        }
        public IActionResult Videos()
        {
            JsonResponse items = LoadJson();
            return View(items);
        }

        public static JsonResponse LoadJson()
        {
            JsonResponse items;
            using (StreamReader r = new StreamReader("./areas.json", Encoding.UTF8))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<JsonResponse>(json);
            }
            return items;
        }
    }

}
