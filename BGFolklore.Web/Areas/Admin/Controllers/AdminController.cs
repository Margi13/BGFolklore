using BGFolklore.Common;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Models.Admin.ViewModels;
using BGFolklore.Services.Admin.Interfaces;
using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Common;
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
        private readonly IManageEventsService manageEventsService;
        private readonly IManageFeedbacksService manageFeedbacksService;

        public AdminController(ILogger<AdminController> logger,
            IWebHostEnvironment webHostEnvironment,
            ITownsService townsService,
            IManageUsersService manageUsersService,
            IManageEventsService manageEventsService,
            IManageFeedbacksService manageFeedbacksService) : base(logger, webHostEnvironment)
        {
            this.manageUsersService = manageUsersService;
            this.manageEventsService = manageEventsService;
            this.manageFeedbacksService = manageFeedbacksService;
            if (Towns.AllTowns is null)
            {
                Towns.GetTowns(townsService);
            }
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //Users
        public IActionResult ShowAllUsers(string sortBy)
        {
            ViewData["UserName"] = sortBy == "UserName" ? "UserName_desc" : "UserName";
            ViewData["CountEvents"] = sortBy == "CountEvents" ? "CountEvents_desc" : "CountEvents";
            ViewData["CountReports"] = sortBy == "CountReports" ? "CountReports_desc" : "CountReports";

            IList<ManageUserViewModel> allUsersViewModel = manageUsersService.GetAllUsers();

            allUsersViewModel = SortViewModel(allUsersViewModel, sortBy);

            return View(allUsersViewModel);
        }
        public IActionResult ManageUser(string userId)
        {
            var userViewModel = manageUsersService.GetUser(userId);

            return View(userViewModel);
        }
        public IActionResult AddToRole(string userId, string roleName)
        {
            ManageUserViewModel userViewModel;
            try
            {
                manageUsersService.AddUserRole(userId, roleName);
                userViewModel = manageUsersService.GetUser(userId);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowAllUsers");
            }
            return View("ManageUser", userViewModel);

        }
        public IActionResult RemoveFromRole(string userId, string roleName)
        {
            ManageUserViewModel userViewModel;
            try
            {
                manageUsersService.RemoveUserRole(userId, roleName);
                userViewModel = manageUsersService.GetUser(userId);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowAllUsers");
            }
            return View("ManageUser", userViewModel);
        }

        //Events
        public IActionResult ShowAllEvents(string sortBy)
        {
            ViewData["UserName"] = sortBy == "UserName" ? "UserName_desc" : "UserName";
            ViewData["EventName"] = sortBy == "EventName" ? "EventName_desc" : "EventName";
            ViewData["CountReports"] = sortBy == "CountReports" ? "CountReports_desc" : "CountReports";

            IList<ManageEventViewModel> allEventsViewModel;
            try
            {
                allEventsViewModel = manageEventsService.GetAllEvents();
                if (allEventsViewModel != null)
                {
                    allEventsViewModel = SortViewModel(allEventsViewModel, sortBy);
                }
            }
            catch (Exception)
            {
                return Error();
            }

            return View(allEventsViewModel);
        }
        public IActionResult ManageEvents(Guid eventId)
        {
            var eventViewModel = manageEventsService.GetEvent(eventId);
            return View(eventViewModel);
        }

        public IActionResult ShowAllFeedbacks(string sortBy)
        {
            ViewData["UserName"] = sortBy == "UserName" ? "UserName_desc" : "UserName";
            ViewData["EventName"] = sortBy == "EventName" ? "EventName_desc" : "EventName";
            ViewData["Status"] = sortBy == "Status" ? "Status_desc" : "Status";

            IList<ManageFeedbackViewModel> allFeedbacksViewModel;
            try
            {
                allFeedbacksViewModel = manageFeedbacksService.GetAllFeedbacks();
                if (allFeedbacksViewModel != null)
                {
                    allFeedbacksViewModel = SortViewModel(allFeedbacksViewModel, sortBy);
                }
            }
            catch (Exception)
            {
                return Error();
            }

            return View(allFeedbacksViewModel);
        }
        public IActionResult ManageFeedbacks(Guid feedId)
        {
            var feedbackViewModel = manageFeedbacksService.GetFeedback(feedId);
            return View(feedbackViewModel);
        }

        private IList<ManageUserViewModel> SortViewModel(IList<ManageUserViewModel> allUsersViewModel, string sortBy)
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
        private IList<ManageEventViewModel> SortViewModel(IList<ManageEventViewModel> allEventsViewModel, string sortBy)
        {
            switch (sortBy)
            {
                case "UserName":
                    allEventsViewModel = allEventsViewModel.OrderBy(e => e.OwnerUserName).ToList();
                    break;
                case "UserName_desc":
                    allEventsViewModel = allEventsViewModel.OrderByDescending(e => e.OwnerUserName).ToList();
                    break;
                case "EventName":
                    allEventsViewModel = allEventsViewModel.OrderBy(e => e.Name).ToList();
                    break;
                case "EventName_desc":
                    allEventsViewModel = allEventsViewModel.OrderByDescending(e => e.Name).ToList();
                    break;
                case "CountReports":
                    allEventsViewModel = allEventsViewModel.OrderBy(e => e.Feedbacks.Count).ToList();
                    break;
                case "CountReports_desc":
                    allEventsViewModel = allEventsViewModel.OrderByDescending(e => e.Feedbacks.Count).ToList();
                    break;
                default:
                    allEventsViewModel = allEventsViewModel.OrderBy(e => e.Name).ToList();
                    break;
            }
            return allEventsViewModel;
        }
        private IList<ManageFeedbackViewModel> SortViewModel(IList<ManageFeedbackViewModel> allEventsViewModel, string sortBy)
        {
            switch (sortBy)
            {
                case "UserName":
                    allEventsViewModel = allEventsViewModel
                        .OrderBy(f => f.OwnerUserName)
                        .ThenBy(f => f.StatusId)
                        .ThenBy(f => f.CreateDateTime)
                        .ToList();
                    break;
                case "UserName_desc":
                    allEventsViewModel = allEventsViewModel
                        .OrderByDescending(f => f.OwnerUserName)
                        .ThenBy(f => f.StatusId)
                        .ThenBy(f => f.CreateDateTime)
                        .ToList();
                    break;
                case "EventName":
                    allEventsViewModel = allEventsViewModel
                        .OrderBy(f => f.EventName)
                        .ThenBy(f => f.StatusId)
                        .ThenBy(f => f.CreateDateTime)
                        .ToList();
                    break;
                case "EventName_desc":
                    allEventsViewModel = allEventsViewModel
                        .OrderByDescending(f => f.EventName)
                        .ThenBy(f => f.StatusId)
                        .ThenBy(f => f.CreateDateTime)
                        .ToList();
                    break;
                case "CountReports":
                    allEventsViewModel = allEventsViewModel
                        .OrderBy(f => f.StatusId)
                        .ThenBy(f => f.CreateDateTime)
                        .ToList();
                    break;
                case "CountReports_desc":
                    allEventsViewModel = allEventsViewModel
                        .OrderByDescending(f => f.StatusId)
                        .ThenBy(f => f.CreateDateTime)
                        .ToList();
                    break;
                default:
                    allEventsViewModel = allEventsViewModel
                        .OrderBy(f => f.CreateDateTime)
                        .ThenBy(f => f.StatusId)
                        .ThenBy(f => f.OwnerUserName)
                        .ToList();
                    break;
            }
            return allEventsViewModel;
        }
    }
}
