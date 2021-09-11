using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using BGFolklore.Services.Public.Interfaces;
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

        public CalendarService(ApplicationDbContext context, IMapper mapper, ITownsService townsService, IFeedbackService feedbackService) : base(context, mapper)
        {
            this.townsService = townsService;
            this.feedbackService = feedbackService;
        }

        public IList<RecurringEventViewModel> GetRecurringEvents()
        {
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays != 0 && e.StatusId != (int)StatusName.Deleted);
            IList<RecurringEventViewModel> recurringEvents = this.Mapper.Map<IList<RecurringEventViewModel>>(publicEvents);
            foreach (var recurringEvent in recurringEvents)
            {
                IList<FeedbackViewModel> feedbacks = feedbackService.GetFeedbackViewModels(recurringEvent.Id);
                recurringEvent.Feedbacks = feedbacks;
            }
            return recurringEvents;
        }

        public IList<UpcomingEventViewModel> GetUpcomingEvents()
        {
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays == 0);

            //Looks if there are old events and delete them. If there are no events to delete - do nothing
            DeleteOldEvents(publicEvents);
            publicEvents = publicEvents.Where(e => e.StatusId != (int)StatusName.Deleted);

            IList<UpcomingEventViewModel> upcomingEvents = this.Mapper.Map<IList<UpcomingEventViewModel>>(publicEvents);
            foreach (var upcomingEvent in upcomingEvents)
            {
                IList<FeedbackViewModel> feedbacks = feedbackService.GetFeedbackViewModels(upcomingEvent.Id);
                upcomingEvent.Feedbacks = feedbacks;
            }
            return upcomingEvents;
        }
        
        public EventViewModel GetEventViewModel(Guid eventId)
        {
            PublicEvent publicEvent = GetPublicEvent(eventId);
            EventViewModel eventViewModel = this.Mapper.Map<EventViewModel>(publicEvent);
            return eventViewModel;
        }

        public void SaveAddEvent(AddEventBindingModel newEvent)
        {
            var newPublicEvent = this.Mapper.Map<PublicEvent>(newEvent);
            newPublicEvent.InsertDateTime = DateTime.Now;

            newPublicEvent = CopyEventInfo(newPublicEvent, newEvent);

            var status = Context.Status.Where(s => s.Id == 1);
            Status newStatus = this.Mapper.Map<Status>(status.First());
            newPublicEvent.Status = newStatus;

            newPublicEvent.Feedbacks = new List<Feedback>();

            this.Context.PublicEvents.Add(newPublicEvent);
            this.Context.SaveChanges();
        }

        public void UpdatePublicEvent(Guid eventId, AddEventBindingModel updatedViewModel)
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

            publicEvent.UpdateDateTime = DateTime.Now;

            this.Context.PublicEvents.Update(publicEvent);
            this.Context.SaveChanges();
        }

        public void DeletePublicEvent(Guid eventId)
        {
            PublicEvent publicEvent = GetPublicEvent(eventId);

            var status = Context.Status.Where(s => s.Id == (int)StatusName.Deleted);
            Status newStatus = this.Mapper.Map<Status>(status.First());

            publicEvent.StatusId = (int)StatusName.Deleted;
            publicEvent.Status = newStatus;

            feedbackService.DeleteAllEventFeedbacks(eventId);

            this.Context.PublicEvents.Update(publicEvent);
            this.Context.SaveChanges();
        }
        public AddEventBindingModel GetBindingModelFromData(Guid eventId)
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

        private PublicEvent GetPublicEvent(Guid eventId)
        {
            var publicEvent = this.Context.PublicEvents.Where(pe => pe.Id == eventId).First();
            PublicEvent findedPublicEvent = this.Mapper.Map<PublicEvent>(publicEvent);
            findedPublicEvent.Feedbacks = feedbackService.GetFeedbacksFromData(findedPublicEvent.Id);

            return findedPublicEvent;
        }

        private PublicEvent CopyEventInfo(PublicEvent destination, AddEventBindingModel source)
        {
            foreach (int attendeeType in source.IntendedFor)
            {
                destination.IntendedFor = destination.IntendedFor | attendeeType;
            }

            if (source.IsRecurring == true)
            {
                foreach (int dayName in source.OccuringDays)
                {
                    destination.OccuringDays = destination.OccuringDays | dayName;
                }
            }

            destination.Town = townsService.GetTownByGivenId(source.TownId);

            return destination;
        }

        private void DeleteOldEvents(IQueryable<PublicEvent> publicEvents)
        {
            var oldEvents = publicEvents.Where(e => e.EventDateTime.CompareTo(DateTime.Now) < 0);
            if (oldEvents != null)
            {
                var eventsToDelete = this.Mapper.Map<IList<PublicEvent>>(oldEvents);
                foreach (var oldEvent in eventsToDelete)
                {
                    DeletePublicEvent(oldEvent.Id);
                }
            }
        }
    }
}
