using BGFolklore.Common;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Models.Admin.ViewModels;
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
    [Authorize(Roles = Constants.AdminRoleName)]

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
        public IActionResult ShowAllUsers(string sortBy)
        {
            ViewData["UserName"] = sortBy == "UserName" ? "UserName_desc" : "UserName";
            ViewData["CountEvents"] = sortBy == "CountEvents" ? "CountEvents_desc" : "CountEvents";
            ViewData["CountReports"] = sortBy == "CountReports" ? "CountReports_desc" : "CountReports";

            IList<ManageUserViewModel> allUsersViewModel = manageUsersService.GetAllUsers();

            allUsersViewModel = SortUserViewModel(allUsersViewModel, sortBy);

            return View(allUsersViewModel);
        }
        public IActionResult AddToRole(string userId, string roleName)
        {
            manageUsersService.AddUserRole(userId, roleName);
            return RedirectToAction("ShowAllUsers");
        }
        public IActionResult RemoveFromRole(string userId, string roleName)
        {
            manageUsersService.RemoveUserRole(userId, roleName);
            return RedirectToAction("ShowAllUsers");
        }
        public IActionResult ManageUser(string userId)
        {
            var userViewModel = manageUsersService.GetUser(userId);

            return View(userViewModel);
        }
        private IList<ManageUserViewModel> SortUserViewModel(IList<ManageUserViewModel> allUsersViewModel,string sortBy)
        {
            switch (sortBy)
            {
                case "UserName":
                    allUsersViewModel = allUsersViewModel.OrderBy(u => u.UserName).ToList();
                    break;
                case "UserName_desc":
                    allUsersViewModel = allUsersViewModel.OrderByDescending(u => u.UserName).ToList();
                    break;
                case "CountEvents":
                    allUsersViewModel = allUsersViewModel.OrderBy(u => u.AllEventsCount).ToList();
                    break;
                case "CountEvents_desc":
                    allUsersViewModel = allUsersViewModel.OrderByDescending(u => u.AllEventsCount).ToList();
                    break;
                case "CountReports":
                    allUsersViewModel = allUsersViewModel.OrderBy(u => u.AllReportsCount).ToList();
                    break;
                case "CountReports_desc":
                    allUsersViewModel = allUsersViewModel.OrderByDescending(u => u.AllReportsCount).ToList();
                    break;
                default:
                    allUsersViewModel = allUsersViewModel.OrderBy(u => u.UserName).ToList();
                    break;
            }
            return allUsersViewModel;
        }
    }
}
