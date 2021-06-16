using BGFolklore.Web.Models;
using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> localizer;

        public HomeController(ILogger<BaseController> logger,
            IStringLocalizer<HomeController> localizer)
            : base(logger)
        {
            this.localizer = localizer;
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger):base

        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult History()
        {
            //string str = localizer["First"]; // Returns right thing
            return View(localizer);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
