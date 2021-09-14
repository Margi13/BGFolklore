using BGFolklore.Data.Models;
using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Common;
using BGFolklore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BGFolklore.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> logger;
        protected readonly IWebHostEnvironment webHostEnvironment;

        protected BaseController(ILogger<BaseController> logger,
            IWebHostEnvironment webHostEnvironment,
            ITownsService townsService)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;

            if (Towns.AllTowns is null)
            {
                Towns.GetTowns(townsService);
            }
        }
        protected BaseController(ILogger<BaseController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
