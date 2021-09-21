using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace BGFolklore.Web.Controllers
{
    //[Authorize]
    public class HomeController : BaseController
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IGalleryService galleryService;

        public HomeController(ILogger<HomeController> logger,
            IWebHostEnvironment webHostEnvironment,
            IGalleryService galleryService) :base(logger,webHostEnvironment)
        {
            _logger = logger;
            this.galleryService = galleryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult History()
        {
            //string str = localizer["First"]; // Returns right thing
            return View();
        }

        public IActionResult Embroidery()
        {
            //string str = localizer["First"]; // Returns right thing
            return View();
        }

        public IActionResult EthnoAreas()
        {
            var ethnoAreas = galleryService.GetAllGalleryViewModels();
            //string str = localizer["First"]; // Returns right thing
            return View(ethnoAreas);
        }
    }
}
