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
        private readonly IRatingService ratingService;
        private readonly UserManager<User> userManager;

        public CalendarController(IWebHostEnvironment webHostEnvironment,
            ICalendarService calendarService,
            IStringLocalizer<CalendarController> localizer,
            IMapper mapper,
            IFeedbackService feedbackService,
            IRatingService ratingService,
            ITownsService townsService,
            UserManager<User> userManager)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.calendarService = calendarService;
            this.localizer = localizer;
            this.mapper = mapper;
            this.townsService = townsService;
            this.feedbackService = feedbackService;
            this.ratingService = ratingService;
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
            try
            {
                IList<UpcomingEventViewModel> viewModelList = calendarService.GetUpcomingEvents();
                var orderedList = viewModelList.OrderBy(ue => ue.EventDateTime);
                if (orderedList == null)
                {
                    throw new Exception();
                }
                PaginatedList<UpcomingEventViewModel> paginatedList = new PaginatedList<UpcomingEventViewModel>(orderedList, pageNumber, 5);
                return View(paginatedList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult RecurringEvents(int pageNumber = 1)
        {
            try
            {
                IList<RecurringEventViewModel> viewModelList = calendarService.GetRecurringEvents();
                var orderedList = viewModelList.OrderBy(re => re.Rating);
                if (orderedList == null)
                {
                    throw new Exception();
                }
                PaginatedList<RecurringEventViewModel> paginatedList = new PaginatedList<RecurringEventViewModel>(orderedList, pageNumber, 5);
                return View(paginatedList);
            }
            catch (Exception)
            {
                throw;
            }
        }

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

                throw;
            }
            return RedirectToAction("RecurringEvents");
        }

        public PartialViewResult MoreInfoBoxPartial(EventViewModel eventViewModel)
        {

            return PartialView("_MoreInfoBoxPartial", new FeedbackViewModel());
        }

        public IActionResult EventFeedbacksPartial(EventViewModel eventViewModel)
        {
            try
            {
                IList<FeedbackViewModel> feedbacks = feedbackService.GetFeedbackViewModels(eventViewModel.Id);
                return View(feedbacks);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IActionResult DeleteFeedback(FeedbackViewModel feedbackViewModel)
        {
            try
            {
                feedbackService.ChangeFeedbackStatus(feedbackViewModel.Id, (int)StatusName.Deleted);
                EventViewModel eventViewModel = calendarService.GetEventViewModel(feedbackViewModel.EventId);
                return RedirectToAction("EventFeedbacksPartial", eventViewModel);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult DeleteAllFeedbacks(FeedbackViewModel feedbackViewModel)
        {
            try
            {
                feedbackService.DeleteAllEventFeedbacks(feedbackViewModel.EventId);
                return RedirectToAction("UpcomingEvents");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult ReadFeedback(FeedbackViewModel feedbackViewModel)
        {
            try
            {
                feedbackService.ChangeFeedbackStatus(feedbackViewModel.Id, (int)StatusName.Readed);
                EventViewModel eventViewModel = calendarService.GetEventViewModel(feedbackViewModel.EventId);
                return RedirectToAction("EventFeedbacksPartial", eventViewModel);
            }
            catch (Exception) { throw; }
        }

        [HttpPost]
        public IActionResult Report(FeedbackBindingModel feedbackBindingModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    feedbackService.SaveFeedback(feedbackBindingModel);

                }
                catch (Exception)
                {

                    throw;
                }
                var pathParts = Request.Headers["Referer"].ToString().Split("/");
                string lastAction = pathParts[pathParts.Length - 1].Split("?")[0];
                return RedirectToAction(lastAction);
            }
            else
            {
                var feedbackViewModel = this.mapper.Map<FeedbackViewModel>(feedbackBindingModel);
                //return PartialView("_MoreInfoBoxPartial", feedbackViewModel);
                var pathParts = Request.Headers["Referer"].ToString().Split("/");
                string lastAction = pathParts[pathParts.Length - 1].Split("?")[0];
                return RedirectToAction(lastAction);
            }
        }

        //Да го направя да взима само ID, а не целия евент, за да мога да го ползвам и в feedbacksPartial
        //Да направя метод за взимане на event по Id
        public IActionResult EditEvent(EventViewModel eventViewModel)
        {
            ViewData["CRUD"] = "Update";
            TempData["Operation"] = "Update";
            try
            {
                AddEventBindingModel addEventBindingModel = calendarService.GetBindingModelFromData(eventViewModel.Id);

                AddEventViewModel viewModel = CreateAddEventViewModel(addEventBindingModel);
                TempData["EventId"] = eventViewModel.Id;
                return View("AddEvent", viewModel);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public IActionResult AddEvent()
        {
            ViewData["CRUD"] = "Create";
            TempData["Operation"] = "Create";
            try
            {
                AddEventViewModel viewModel = CreateAddEventViewModel();
                return View(viewModel);
            }
            catch (Exception)
            {
                throw;
            }
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
                    throw;
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
                var addEventViewModel = CreateAddEventViewModel(addEventBindingModel);

                return View(addEventViewModel);
            }
        }

        public IActionResult DeleteEvent(EventViewModel eventViewModel)
        {
            try
            {
                calendarService.DeletePublicEvent(eventViewModel.Id);
            }
            catch (Exception)
            {
                throw;
            }
            var pathParts = Request.Headers["Referer"].ToString().Split("/");
            string lastAction = pathParts[pathParts.Length - 1].Split("?")[0];
            return RedirectToAction(lastAction);
        }

        private AddEventViewModel CreateAddEventViewModel()
        {
            var viewModel = new AddEventViewModel();

            viewModel.IntendedFor = new List<SelectListItem>();
            GetAttendeeType(viewModel);

            viewModel.OccuringDays = new List<SelectListItem>();
            GetOccuringDays(viewModel);

            return viewModel;
        }
        private AddEventViewModel CreateAddEventViewModel(AddEventBindingModel addEventBindingModel)
        {
            var viewModel = this.mapper.Map<AddEventViewModel>(addEventBindingModel);

            viewModel.IntendedFor = new List<SelectListItem>();
            GetSelectedAttendeeType(viewModel, addEventBindingModel);

            viewModel.OccuringDays = new List<SelectListItem>();
            GetSelectedOccuringDays(viewModel, addEventBindingModel);

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
