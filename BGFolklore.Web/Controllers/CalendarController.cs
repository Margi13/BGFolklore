using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Common;
using Microsoft.AspNetCore.Hosting;
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

        public CalendarController(IWebHostEnvironment webHostEnvironment,
            ICalendarService calendarService,
            IStringLocalizer<CalendarController> localizer,
            IMapper mapper,
            ITownsService townsService)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.calendarService = calendarService;
            this.localizer = localizer;
            this.mapper = mapper;
            this.townsService = townsService;
        }
        public IActionResult Index()
        {
            if (Towns.AllTowns is null)
            {
                Towns.GetTowns(townsService);
            }

            return View();
        }
        public IActionResult UpcomingEvents()
        {
            IList<UpcomingEventViewModel> viewModelList = calendarService.GetUpcomingEvents();
            return View(viewModelList);
        }
        public IActionResult RecurringEvents()
        {
            IList<RecurringEventViewModel> viewModelList = calendarService.GetRecurringEvents();
            return View(viewModelList);
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
                return RedirectToAction("UpcomingEvents");
            }
            else
            {
                var addEventViewModel = this.mapper.Map<AddEventViewModel>(addEventBindingModel);

                addEventViewModel.IntendedFor = new List<SelectListItem>();
                //Execute only GetSelectedAttendeeType which calls GetAttendeeType
                GetAttendeeType(addEventViewModel);
                GetSelectedAttendeeType(addEventViewModel, addEventBindingModel);

                addEventViewModel.OccuringDays = new List<SelectListItem>();
                GetOccuringDays(addEventViewModel);
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
            if (bindingModel.OccuringDays != null)
            {
                foreach (var selectedDay in bindingModel.OccuringDays)
                {
                    foreach (var dayName in viewModel.OccuringDays)
                    {
                        if (dayName.Value.CompareTo(selectedDay.ToString()) == 0)
                        {
                            dayName.Selected = true;
                        }
                        else
                        {
                            dayName.Selected = false;
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
            //Add here GetAttendeeType execution?
            if (bindingModel.IntendedFor != null)
            {
                foreach (var selectedAttendee in bindingModel.IntendedFor)
                {
                    foreach (var attendee in viewModel.IntendedFor)
                    {
                        if (attendee.Value.CompareTo(selectedAttendee.ToString()) == 0)
                        {
                            attendee.Selected = true;
                        }
                        else
                        {
                            attendee.Selected = false;
                        }
                    }
                }
            }
        }
    }
}
