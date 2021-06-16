using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> logger;
        //protected readonly IStringLocalizer<BaseController> localizer;

        protected BaseController(ILogger<BaseController> logger)
        {
            this.logger = logger;
            //this.localizer = localizer;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
