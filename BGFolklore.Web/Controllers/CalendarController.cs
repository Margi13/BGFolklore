using System;
using System.Collections.Generic;
using AutoMapper;
using BGFolklore.Common.Common;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Models;
using BGFolklore.Web.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace BGFolklore.Web.Controllers
{
    [Authorize]
    public class CalendarController : BaseController
    {
        private readonly IStringLocalizer<CalendarController> localizer;
        private readonly IMapper mapper;
        private readonly ICalendarService calendarService;
        private readonly IFeedbackService feedbackService;
        private readonly IRatingService ratingService;
        private readonly UserManager<User> userManager;
        private ViewModelHelper helperMethods;
        public CalendarController(ILogger<BaseController> logger, IWebHostEnvironment webHostEnvironment,
            IStringLocalizer<CalendarController> localizer, ITownsService townsService, IMapper mapper,
            ICalendarService calendarService, IFeedbackService feedbackService, IRatingService ratingService,
            UserManager<User> userManager) : base(logger, webHostEnvironment, townsService)
        {
            this.localizer = localizer;
            this.mapper = mapper;
            this.calendarService = calendarService;
            this.feedbackService = feedbackService;
            this.ratingService = ratingService;
            this.userManager = userManager;

            this.helperMethods = new ViewModelHelper(localizer, mapper);
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        //Event Actions
        [AllowAnonymous]
        public IActionResult UpcomingEvents(int pageNumber = 1)
        {
            FilterEventsViewModel viewModel = new FilterEventsViewModel
            {
                Filters = helperMethods.CreateFilterViewModel()
            };
            try
            {
                IList<UpcomingEventViewModel> orderedList = calendarService.GetUpcomingEvents(null);
                PaginatedList<UpcomingEventViewModel> paginatedList = new PaginatedList<UpcomingEventViewModel>(orderedList, pageNumber, 5);
                viewModel.UpcomingPaginatedList = paginatedList;
                viewModel.Filters.IsRecurring = false;
            }
            catch (Exception)
            {
                return Error();
            }
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult UpcomingEvents(FilterBindingModel filterBindingModel)
        {
            FilterEventsViewModel viewModel = new FilterEventsViewModel
            {
                Filters = helperMethods.CreateFilterViewModel()
            };
            viewModel.Filters.IsRecurring = false;

            IList<UpcomingEventViewModel> orderedList;
            bool hasGivenFilter = true;

            if (filterBindingModel.AreaId == 0 &&
                filterBindingModel.TownId == 0 &&
                filterBindingModel.OwnerId == null &&
                filterBindingModel.IntendedFor == null &&
                filterBindingModel.PlaceType == null &&
                filterBindingModel.AfterDate == null &&
                filterBindingModel.BeforeDate == null &&
                filterBindingModel.DurationInDays == 0
                )
            {
                hasGivenFilter = false;
            }

            if (ModelState.IsValid)
            {
                if (hasGivenFilter)
                {
                    orderedList = calendarService.GetUpcomingEvents(filterBindingModel);
                }
                else
                {
                    orderedList = calendarService.GetUpcomingEvents(null);
                }
                PaginatedList<UpcomingEventViewModel> paginatedList = new PaginatedList<UpcomingEventViewModel>(orderedList, 1, 5);
                viewModel.UpcomingPaginatedList = paginatedList;
                viewModel.Filters = helperMethods.CreateFilterViewModel();
                viewModel.Filters.IsRecurring = false;
                return View(viewModel);
            }
            else
            {
                orderedList = calendarService.GetUpcomingEvents(null);
                PaginatedList<UpcomingEventViewModel> paginatedList = new PaginatedList<UpcomingEventViewModel>(orderedList, 1, 5);
                viewModel.UpcomingPaginatedList = paginatedList;
                viewModel.Filters = helperMethods.CreateFilterViewModel(filterBindingModel);
                viewModel.Filters.IsRecurring = false;

                return View(viewModel);
            }

        }

        [AllowAnonymous]
        public IActionResult RecurringEvents(int pageNumber = 1)
        {
            FilterEventsViewModel viewModel = new FilterEventsViewModel
            {
                Filters = helperMethods.CreateFilterViewModel()
            };
            viewModel.Filters.IsRecurring = true;
            try
            {
                IList<RecurringEventViewModel> orderedList = calendarService.GetRecurringEvents(null);
                PaginatedList<RecurringEventViewModel> paginatedList = new PaginatedList<RecurringEventViewModel>(orderedList, pageNumber, 5);
                viewModel.RecurringPaginatedList = paginatedList;
            }
            catch (Exception)
            {
                return Error();
            }
            return View(viewModel);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public IActionResult RecurringEvents(FilterBindingModel filterBindingModel)
        {
            FilterEventsViewModel viewModel = new FilterEventsViewModel
            {
                Filters = helperMethods.CreateFilterViewModel()
            };
            viewModel.Filters.IsRecurring = true;
            IList<RecurringEventViewModel> orderedList;
            bool hasGivenFilter = true;

            if (filterBindingModel.AreaId == 0 &&
                filterBindingModel.TownId == 0 &&
                filterBindingModel.OwnerId == null &&
                filterBindingModel.IntendedFor == null &&
                filterBindingModel.OccuringDays == null &&
                filterBindingModel.PlaceType == null &&
                filterBindingModel.AfterDate == null &&
                filterBindingModel.BeforeDate == null
                )
            {
                hasGivenFilter = false;
            }

            if (ModelState.IsValid)
            {
                if (hasGivenFilter)
                {
                    orderedList = calendarService.GetRecurringEvents(filterBindingModel);
                }
                else
                {
                    orderedList = calendarService.GetRecurringEvents(null);
                }
                PaginatedList<RecurringEventViewModel> paginatedList = new PaginatedList<RecurringEventViewModel>(orderedList, 1, 5);
                viewModel.RecurringPaginatedList = paginatedList;
                viewModel.Filters = helperMethods.CreateFilterViewModel();
                viewModel.Filters.IsRecurring = true;
                return View(viewModel);
            }
            else
            {
                orderedList = calendarService.GetRecurringEvents(null);
                PaginatedList<RecurringEventViewModel> paginatedList = new PaginatedList<RecurringEventViewModel>(orderedList, 1, 5);
                viewModel.RecurringPaginatedList = paginatedList;
                viewModel.Filters = helperMethods.CreateFilterViewModel(filterBindingModel);
                viewModel.Filters.IsRecurring = true;

                return View(viewModel);
            }
        }

        public IActionResult EditEvent(Guid eventId)
        {
            //ViewData["CRUD"] = "Update";
            TempData["Operation"] = "Update";

            AddEventViewModel viewModel;
            try
            {
                AddEventBindingModel addEventBindingModel = calendarService.GetBindingModelFromData(eventId);

                viewModel = helperMethods.CreateAddEventViewModel(addEventBindingModel);
                TempData["EventId"] = eventId;
            }
            catch (Exception)
            {
                return Error();
            }

            if (userManager.GetUserId(User).Equals(viewModel.OwnerId))
            {
                return View("AddEvent", viewModel);
            }
            else
            {
                return AddEvent();
            }

        }

        [HttpGet]
        public IActionResult AddEvent()
        {
            ViewData["CRUD"] = "Create";
            TempData["Operation"] = "Create";
            AddEventViewModel viewModel;
            try
            {
                viewModel = helperMethods.CreateAddEventViewModel();
            }
            catch (Exception)
            {
                return Error();
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddEvent(AddEventBindingModel addEventBindingModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (TempData["Operation"].Equals("Create"))
                    {
                        calendarService.SaveAddEvent(addEventBindingModel);
                    }
                    else if (TempData["Operation"].Equals("Update"))
                    {
                        Guid eventId = (Guid)TempData["EventId"];
                        calendarService.UpdatePublicEvent(eventId, addEventBindingModel);
                    }
                }
                catch (Exception)
                {
                    return Error();
                }
                if (addEventBindingModel.IsRecurring)
                {
                    return RedirectToAction("RecurringEvents");
                }
                else
                {
                    return RedirectToAction("UpcomingEvents");
                }
            }
            else
            {
                var addEventViewModel = helperMethods.CreateAddEventViewModel(addEventBindingModel);

                return View(addEventViewModel);
            }
        }

        public IActionResult DeleteEvent(Guid eventId)
        {
            try
            {
                var eventToDelete = calendarService.GetEventViewModel(eventId);
                if (userManager.GetUserId(User).Equals(eventToDelete.OwnerId))
                {
                    calendarService.DeletePublicEvent(eventId);
                }
            }
            catch (Exception)
            {
                return Error();
            }
            var pathParts = Request.Headers["Referer"].ToString().Split("/");
            string lastAction = pathParts[pathParts.Length - 1].Split("?")[0];
            return RedirectToAction(lastAction);
        }

        // ModalBox Actions
        [HttpPost]
        public IActionResult RateForEvent(int rate, Guid eventId)
        {
            try
            {
                RatingBindingModel ratingBindingModel = new RatingBindingModel();
                ratingBindingModel.OwnerId = userManager.GetUserId(User);
                ratingBindingModel.EventId = eventId;
                ratingBindingModel.Rate = rate;
                ratingService.SaveRating(ratingBindingModel);

            }
            catch (Exception)
            {
                return Error();
            }
            return RedirectToAction("RecurringEvents");
        }

        public IActionResult OwnEvents()
        {
            OwnerEventsViewModel ownEventsViewModel;
            try
            {
                string userId = userManager.GetUserId(User);
                ownEventsViewModel = calendarService.GetAllEventsForUser(userId);
            }
            catch (Exception)
            {

                return Error();
            }

            return View(ownEventsViewModel);
        }
    }
}
