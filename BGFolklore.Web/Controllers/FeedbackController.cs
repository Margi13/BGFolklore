using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Controllers
{
    public class FeedbackController : BaseController
    {
        private readonly IMapper mapper;
        private readonly ICalendarService calendarService;
        private readonly IFeedbackService feedbackService;
        private readonly UserManager<User> userManager;

        public FeedbackController(ILogger<BaseController> logger, IWebHostEnvironment webHostEnvironment, IMapper mapper,
            ICalendarService calendarService, IFeedbackService feedbackService, UserManager<User> userManager) : base(logger, webHostEnvironment)
        {
            this.mapper = mapper;
            this.calendarService = calendarService;
            this.feedbackService = feedbackService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult EventFeedbacks(Guid eventId)
        {
            IList<FeedbackViewModel> feedbacks;
            EventViewModel eventViewModel;
            try
            {
                eventViewModel = calendarService.GetEventViewModel(eventId);
                feedbacks = feedbackService.GetFeedbackViewModels(eventId);
            }
            catch (Exception)
            {
                return Error();
            }
            ViewData["EventName"] = eventViewModel.Name;
            ViewData["EventOwner"] = eventViewModel.OwnerId;
            return View("EventFeedbacks", feedbacks);

        }
        public IActionResult DeleteFeedback(Guid feedId, Guid eventId)
        {
            EventViewModel eventViewModel;
            try
            {
                eventViewModel = calendarService.GetEventViewModel(eventId);

                if (userManager.GetUserId(User).Equals(eventViewModel.OwnerId))
                {
                    feedbackService.ChangeFeedbackStatus(feedId, (int)StatusName.Deleted);
                    eventViewModel = calendarService.GetEventViewModel(eventId);
                }
            }
            catch (Exception)
            {
                return Error();
            }
            return EventFeedbacks(eventId);

        }
        public IActionResult DeleteAllFeedbacks(Guid eventId)
        {
            EventViewModel eventViewModel;
            try
            {
                eventViewModel = calendarService.GetEventViewModel(eventId);

                if (userManager.GetUserId(User).Equals(eventViewModel.OwnerId))
                {
                    feedbackService.DeleteAllEventFeedbacks(eventId);
                    eventViewModel = calendarService.GetEventViewModel(eventId);
                }
            }
            catch (Exception)
            {
                return Error();
            }
            return RedirectToAction("Index","Calendar");
        }
        public IActionResult ReadFeedback(Guid feedId, Guid eventId)
        {

            EventViewModel eventViewModel;
            try
            {
                eventViewModel = calendarService.GetEventViewModel(eventId);
                if (userManager.GetUserId(User).Equals(eventViewModel.OwnerId))
                {
                    feedbackService.ChangeFeedbackStatus(feedId, (int)StatusName.Readed);
                    eventViewModel = calendarService.GetEventViewModel(eventId);
                }
            }
            catch (Exception)
            {
                return Error();
            }
            return EventFeedbacks(eventId);
        }
        [HttpPost]
        public IActionResult AddFeedback(FeedbackBindingModel feedbackBindingModel)
        {
            if (ModelState.IsValid)
            {
                AddEventBindingModel eventViewModel;
                try
                {
                    eventViewModel = calendarService.GetBindingModelFromData(feedbackBindingModel.EventId);
                    if (userManager.GetUserId(User).Equals(feedbackBindingModel.OwnerId))
                    {
                        feedbackService.SaveFeedback(feedbackBindingModel);
                    }

                }
                catch (Exception)
                {
                    return Error();
                }

                if (eventViewModel.IsRecurring)
                {
                    return RedirectToAction("RecurringEvents", "Calendar");
                }
                else
                {
                    return RedirectToAction("UpcomingEvents", "Calendar");
                }
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

    }
}
