using BGFolklore.Common;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Services.Admin.Interfaces;
using BGFolklore.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles = CustomRoleNames.Administrator)]
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName + "," + Constants.ModeratorRoleName)]

    public class AdminController : BaseController
    {
        private readonly IManageUsersService manageUsersService;

        public AdminController(ILogger<AdminController> logger,
            IWebHostEnvironment webHostEnvironment,
            IManageUsersService manageUsersService) : base(logger, webHostEnvironment)
        {
            this.manageUsersService = manageUsersService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult ShowAllEvents()
        {
            return View();
        }

        [Authorize(Roles = Constants.AdminRoleName)]
        public IActionResult ShowAllUsers()
        {
            var users = manageUsersService.GetAllUsers();
            return View();
        }
    }
}
