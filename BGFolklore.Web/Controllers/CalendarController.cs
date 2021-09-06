using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BGFolklore.Web.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICalendarService calendarService;
        private readonly IStringLocalizer<CalendarController> localizer;
        private readonly IMapper mapper;
        private readonly ITownsService townsService;
        private readonly IFeedbackService feedbackService;
        private readonly UserManager<User> userManager;

        public CalendarController(IWebHostEnvironment webHostEnvironment,
            ICalendarService calendarService,
            IStringLocalizer<CalendarController> localizer,
            IMapper mapper,
            ITownsService townsService,
            IFeedbackService feedbackService,
            UserManager<User> userManager)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.calendarService = calendarService;
            this.localizer = localizer;
            this.mapper = mapper;
            this.townsService = townsService;
            this.feedbackService = feedbackService;
            this.userManager = userManager;
            if (Towns.AllTowns is null)
            {
                Towns.GetTowns(townsService);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpcomingEvents(int pageNumber = 1)
        {
            IList<UpcomingEventViewModel> viewModelList = calendarService.GetUpcomingEvents();
            var orderedList = viewModelList.OrderBy(ue => ue.EventDateTime);
            PaginatedList<UpcomingEventViewModel> paginatedList = new PaginatedList<UpcomingEventViewModel>(orderedList, pageNumber, 5);
            return View(paginatedList);
        }

        public IActionResult RecurringEvents(int pageNumber = 1)
        {
            IList<RecurringEventViewModel> viewModelList = calendarService.GetRecurringEvents();
            var orderedList = viewModelList.OrderBy(re => re.Rating);
            PaginatedList<RecurringEventViewModel> paginatedList = new PaginatedList<RecurringEventViewModel>(orderedList, pageNumber, 5);
            return View(paginatedList);
        }

        public IActionResult ModalPartial(EventViewModel eventViewModel)
        {
            
            return PartialView("_ModalBoxPartial", new FeedbackViewModel());
        }


        [HttpPost]
        public IActionResult Report(FeedbackBindingModel feedbackBindingModel)
        {
            if (ModelState.IsValid)
            {
                feedbackService.SaveFeedback(feedbackBindingModel);
                var pathParts = Request.Headers["Referer"].ToString().Split("/");
                string lastAction = pathParts[pathParts.Length - 1];
                return RedirectToAction(lastAction);
            }
            else
            {
                var feedbackViewModel = this.mapper.Map<FeedbackViewModel>(feedbackBindingModel);

                var pathParts = Request.Headers["Referer"].ToString().Split("/");
                string lastAction = pathParts[pathParts.Length - 1];
                return RedirectToAction(lastAction, feedbackViewModel);
            }
        }

        public IActionResult DeleteEvent(EventViewModel eventViewModel)
        {
            calendarService.DeletePublicEvent(eventViewModel);
            return Ok();
        }

        [HttpGet]
        public IActionResult AddEvent()
        {
            AddEventViewModel viewModel = GetAddEventViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddEvent(AddEventBindingModel addEventBindingModel)
        {
            if (ModelState.IsValid)
            {
                calendarService.SaveAddEvent(addEventBindingModel);
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
                var addEventViewModel = this.mapper.Map<AddEventViewModel>(addEventBindingModel);

                addEventViewModel.IntendedFor = new List<SelectListItem>();
                GetSelectedAttendeeType(addEventViewModel, addEventBindingModel);

                addEventViewModel.OccuringDays = new List<SelectListItem>();
                GetSelectedOccuringDays(addEventViewModel, addEventBindingModel);

                return View(addEventViewModel);
            }
        }

        private AddEventViewModel GetAddEventViewModel()
        {
            var viewModel = new AddEventViewModel();

            viewModel.IntendedFor = new List<SelectListItem>();
            GetAttendeeType(viewModel);

            viewModel.OccuringDays = new List<SelectListItem>();
            GetOccuringDays(viewModel);

            return viewModel;
        }

        private void GetOccuringDays(AddEventViewModel viewModel)
        {
            foreach (var dayName in Enum.GetValues(typeof(DaysOfWeek)))
            {
                var selectListItem = new SelectListItem();
                selectListItem.Value = ((int)dayName).ToString();
                selectListItem.Text = localizer[$"Day{dayName}"];
                viewModel.OccuringDays.Add(selectListItem);
            }
        }

        private void GetSelectedOccuringDays(AddEventViewModel viewModel, AddEventBindingModel bindingModel)
        {
            GetOccuringDays(viewModel);

            if (bindingModel.OccuringDays != null)
            {

                foreach (var dayName in viewModel.OccuringDays)
                {
                    dayName.Selected = false;

                    foreach (var selectedDay in bindingModel.OccuringDays)
                    {
                        if (dayName.Value.CompareTo(selectedDay.ToString()) == 0)
                        {
                            dayName.Selected = true;
                        }
                    }
                }
            }
        }

        private void GetAttendeeType(AddEventViewModel viewModel)
        {
            foreach (var type in Enum.GetValues(typeof(AttendeeType)))
            {
                var selectListItem = new SelectListItem();
                selectListItem.Value = ((int)type).ToString();
                selectListItem.Text = localizer[$"AttendeeType{type}"];
                viewModel.IntendedFor.Add(selectListItem);
            }
        }

        private void GetSelectedAttendeeType(AddEventViewModel viewModel, AddEventBindingModel bindingModel)
        {
            GetAttendeeType(viewModel);

            if (bindingModel.IntendedFor != null)
            {
                foreach (var attendee in viewModel.IntendedFor)
                {
                    attendee.Selected = false;

                    foreach (var selectedAttendee in bindingModel.IntendedFor)
                    {
                        if (attendee.Value.CompareTo(selectedAttendee.ToString()) == 0)
                        {
                            attendee.Selected = true;
                        }
                    }
                }
            }
        }
    }
}
