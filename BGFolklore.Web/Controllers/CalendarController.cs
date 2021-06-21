using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UpcomingEvents()
        {
            return View();
        }
        public IActionResult AllPlaces()
        {
            return View();
        }
    }
}
