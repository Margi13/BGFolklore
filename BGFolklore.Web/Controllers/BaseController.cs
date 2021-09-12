using BGFolklore.Data.Models;
using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BGFolklore.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> logger;
        protected readonly IWebHostEnvironment webHostEnvironment;

        protected readonly IStringLocalizer<BaseController> localizer;

        protected BaseController(ILogger<BaseController> logger,
            IWebHostEnvironment webHostEnvironment,
            IStringLocalizer<CalendarController> localizer,
            ITownsService townsService)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.localizer = localizer;

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
    }
}
