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
        public HomeController(ILogger<HomeController> logger,
            IWebHostEnvironment webHostEnvironment) :base(logger,webHostEnvironment)
        {
            _logger = logger;
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
            //string str = localizer["First"]; // Returns right thing
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
