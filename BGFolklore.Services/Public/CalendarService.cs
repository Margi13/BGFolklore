using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data;
using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public
{
    public class CalendarService : BaseService, ICalendarService
    {
        private readonly ITownsService townsService;
        private readonly IFeedbackService feedbackService;
        private readonly IRatingService ratingService;
        private readonly IFilterService filterService;
        private readonly IStatusService statusService;
        private readonly UserManager<User> userManager;

        public CalendarService(ApplicationDbContext context, IMapper mapper,
            ITownsService townsService,
            IFeedbackService feedbackService,
            IRatingService ratingService,
            IFilterService filterService,
            IStatusService statusService,
            UserManager<User> userManager) : base(context, mapper)
        {
            this.townsService = townsService;
            this.feedbackService = feedbackService;
            this.ratingService = ratingService;
            this.filterService = filterService;
            this.statusService = statusService;
            this.userManager = userManager;
        }

        public IList<RecurringEventViewModel> GetRecurringEvents(FilterBindingModel filterBindingModel)
        {
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays != 0 && e.StatusId != (int)StatusName.Deleted);
            if (publicEvents == null)
            {
                throw new Exception();
            }

            IList<RecurringEventViewModel> recurringEvents = new List<RecurringEventViewModel>();
            try
            {
                if (filterBindingModel != null)
                {
                    //Ordering is called in the method
                    recurringEvents = filterService.GetFilteredRecurringEvents(filterBindingModel, publicEvents);
                }
                else
                {
                    var ordered = filterService.OrderFilteredData(null, true, publicEvents);
                    if (ordered != null)
                    {
                        recurringEvents = this.Mapper.Map<IList<RecurringEventViewModel>>(publicEvents);
                    }
                }

                foreach (var recurringEvent in recurringEvents)
                {
                    IList<FeedbackViewModel> feedbacks = feedbackService.GetFeedbackViewModels(recurringEvent.Id);
                    if (feedbacks == null)
                    {
                        throw new Exception();
                    }
                    recurringEvent.Feedbacks = feedbacks;

                    recurringEvent.Rating = ratingService.GetEventRatingsAverage(recurringEvent.Id);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return recurringEvents;
        }

        public IList<UpcomingEventViewModel> GetUpcomingEvents(FilterBindingModel filterBindingModel)
        {
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays == 0);
            if (publicEvents == null)
            {
                throw new Exception();
            }
            IList<UpcomingEventViewModel> upcomingEvents = new List<UpcomingEventViewModel>();
            try
            {
                //Looks if there are old events and delete them. If there are no events to delete - do nothing
                DeleteOldEvents(publicEvents);
                publicEvents = publicEvents.Where(e => e.StatusId != (int)StatusName.Deleted);
                if (publicEvents != null)
                {
                    if (filterBindingModel != null)
                    {
                        //Ordering is called in the method
                        upcomingEvents = filterService.GetFilteredUpcomingEvents(filterBindingModel, publicEvents);
                    }
                    else
                    {
                        var ordered = filterService.OrderFilteredData(null, false, publicEvents);
                        if (ordered != null)
                        {
                            upcomingEvents = this.Mapper.Map<IList<UpcomingEventViewModel>>(publicEvents);
                        }
                    }

                    foreach (var upcomingEvent in upcomingEvents)
                    {
                        IList<FeedbackViewModel> feedbacks = feedbackService.GetFeedbackViewModels(upcomingEvent.Id);
                        upcomingEvent.Feedbacks = feedbacks;
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return upcomingEvents;
        }

        public void SaveAddEvent(AddEventBindingModel newEvent)
        {
            try
            {
                var newPublicEvent = this.Mapper.Map<PublicEvent>(newEvent);
                newPublicEvent.InsertDateTime = DateTime.Now;
                newPublicEvent.Feedbacks = new List<Feedback>();

                newPublicEvent = CopyEventInfo(newPublicEvent, newEvent);

                newPublicEvent.Town = townsService.GetTownByGivenId(newEvent.TownId);
                newPublicEvent.Status = statusService.GetStatus((int)StatusName.New);

                this.Context.PublicEvents.Add(newPublicEvent);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public EventViewModel GetEventViewModel(Guid eventId)
        {
            EventViewModel eventViewModel;
            try
            {
                PublicEvent publicEvent = GetPublicEvent(eventId);
                eventViewModel = this.Mapper.Map<EventViewModel>(publicEvent);
            }
            catch (Exception)
            {
                throw;
            }
            return eventViewModel;
        }

        public AddEventBindingModel GetBindingModelFromData(Guid eventId)
        {
            try
            {
                PublicEvent publicEvent = GetPublicEvent(eventId);
                AddEventBindingModel bindingModel = this.Mapper.Map<AddEventBindingModel>(publicEvent);

                var helper = new List<int>();
                foreach (int attendeeType in Enum.GetValues(typeof(AttendeeType)))
                {
                    if ((publicEvent.IntendedFor & attendeeType) == attendeeType)
                    {
                        helper.Add(attendeeType);
                    }
                }
                bindingModel.IntendedFor = helper.ToArray();
                helper.Clear();

                if (publicEvent.OccuringDays != 0)
                {
                    bindingModel.IsRecurring = true;

                    foreach (int dayName in Enum.GetValues(typeof(DaysOfWeek)))
                    {
                        if ((publicEvent.OccuringDays & dayName) == dayName)
                        {
                            helper.Add(dayName);
                        }
                    }
                    bindingModel.OccuringDays = helper.ToArray();
                    helper.Clear();
                }

                return bindingModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePublicEvent(Guid eventId, AddEventBindingModel updatedViewModel)
        {
            try
            {
                PublicEvent publicEvent = GetPublicEvent(eventId);
                publicEvent.Name = updatedViewModel.Name;
                publicEvent.EventDateTime = updatedViewModel.EventDateTime;
                publicEvent.Address = updatedViewModel.Address;
                publicEvent.Description = updatedViewModel.Description;
                publicEvent.DurationInDays = updatedViewModel.DurationInDays;
                publicEvent.Phone = updatedViewModel.Phone;
                publicEvent.PlaceType = updatedViewModel.PlaceType;

                CopyEventInfo(publicEvent, updatedViewModel);

                publicEvent.Town = townsService.GetTownByGivenId(updatedViewModel.TownId);

                publicEvent.UpdateDateTime = DateTime.Now;

                this.Context.PublicEvents.Update(publicEvent);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePublicEvent(Guid eventId)
        {
            try
            {
                Status newStatus = statusService.GetStatus((int)StatusName.Deleted);
                PublicEvent publicEvent = GetPublicEvent(eventId);

                publicEvent.StatusId = (int)StatusName.Deleted;
                publicEvent.Status = newStatus;

                feedbackService.DeleteAllEventFeedbacks(eventId);

                this.Context.PublicEvents.Update(publicEvent);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private PublicEvent GetPublicEvent(Guid eventId)
        {
            var publicEvent = this.Context.PublicEvents.Where(pe => pe.Id == eventId).FirstOrDefault();
            if (publicEvent == null)
            {
                throw new Exception();
            }

            PublicEvent findedPublicEvent;

            try
            {
                findedPublicEvent = this.Mapper.Map<PublicEvent>(publicEvent);
                findedPublicEvent.Feedbacks = feedbackService.GetFeedbacksFromData(findedPublicEvent.Id);
                findedPublicEvent.Town = townsService.GetTownByGivenId(findedPublicEvent.TownId);
                findedPublicEvent.Status = statusService.GetStatus(findedPublicEvent.StatusId);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return findedPublicEvent;
        }

        private PublicEvent CopyEventInfo(PublicEvent destination, AddEventBindingModel source)
        {
            destination.IntendedFor = 0;
            foreach (int attendeeType in source.IntendedFor)
            {
                destination.IntendedFor = destination.IntendedFor | attendeeType;
            }

            if (source.IsRecurring == true)
            {
                destination.OccuringDays = 0;
                foreach (int dayName in source.OccuringDays)
                {
                    destination.OccuringDays = destination.OccuringDays | dayName;
                }
            }

            return destination;
        }

        private void DeleteOldEvents(IQueryable<PublicEvent> publicEvents)
        {
            var oldEvents = publicEvents.Where(e => e.EventDateTime.CompareTo(DateTime.Now) < 0 && e.StatusId != (int)StatusName.Deleted);
            if (oldEvents == null)
            {
                throw new Exception();
            }
            IList<PublicEvent> eventsToDelete;
            try
            {
                eventsToDelete = this.Mapper.Map<IList<PublicEvent>>(oldEvents);
                foreach (var oldEvent in eventsToDelete)
                {
                    DeletePublicEvent(oldEvent.Id);
                }
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }
    }
}
