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
        private readonly IRatingService ratingService;

        public CalendarService(ApplicationDbContext context, IMapper mapper,
            ITownsService townsService,
            IFeedbackService feedbackService,
            IRatingService ratingService) : base(context, mapper)
        {
            this.townsService = townsService;
            this.feedbackService = feedbackService;
            this.ratingService = ratingService;
        }

        public IList<RecurringEventViewModel> GetRecurringEvents()
        {
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays != 0 && e.StatusId != (int)StatusName.Deleted);
            if (publicEvents == null)
            {
                throw new Exception();
            }
            IList<RecurringEventViewModel> recurringEvents = this.Mapper.Map<IList<RecurringEventViewModel>>(publicEvents);
            try
            {
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

        public IList<UpcomingEventViewModel> GetUpcomingEvents()
        {
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays == 0);
            if (publicEvents == null)
            {
                throw new Exception();
            }

            try
            {
                //Looks if there are old events and delete them. If there are no events to delete - do nothing
                DeleteOldEvents(publicEvents);
                publicEvents = publicEvents.Where(e => e.StatusId != (int)StatusName.Deleted);
                if (publicEvents != null)
                {
                    IList<UpcomingEventViewModel> upcomingEvents = this.Mapper.Map<IList<UpcomingEventViewModel>>(publicEvents);
                    foreach (var upcomingEvent in upcomingEvents)
                    {
                        IList<FeedbackViewModel> feedbacks = feedbackService.GetFeedbackViewModels(upcomingEvent.Id);
                        upcomingEvent.Feedbacks = feedbacks;
                    }
                    return upcomingEvents;
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

        }

        public EventViewModel GetEventViewModel(Guid eventId)
        {
            try
            {
                PublicEvent publicEvent = GetPublicEvent(eventId);
                EventViewModel eventViewModel = this.Mapper.Map<EventViewModel>(publicEvent);
                return eventViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveAddEvent(AddEventBindingModel newEvent)
        {
            var newPublicEvent = this.Mapper.Map<PublicEvent>(newEvent);
            newPublicEvent.InsertDateTime = DateTime.Now;

            newPublicEvent = CopyEventInfo(newPublicEvent, newEvent);

            newPublicEvent.Feedbacks = new List<Feedback>();

            var status = Context.Status.Where(s => s.Id == (int)StatusName.New).FirstOrDefault();
            if (status == null)
            {
                throw new Exception();
            }
            Status newStatus = this.Mapper.Map<Status>(status);
            newPublicEvent.Status = newStatus;

            try
            {
                this.Context.PublicEvents.Add(newPublicEvent);
                this.Context.SaveChanges();
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
            var status = Context.Status.Where(s => s.Id == (int)StatusName.Deleted).FirstOrDefault();
            if (status == null)
            {
                throw new Exception();
            }
            Status newStatus = this.Mapper.Map<Status>(status);
            try
            {
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

        private PublicEvent GetPublicEvent(Guid eventId)
        {
            var publicEvent = this.Context.PublicEvents.Where(pe => pe.Id == eventId).FirstOrDefault();
            if (publicEvent == null)
            {
                throw new Exception();
            }

            PublicEvent findedPublicEvent = this.Mapper.Map<PublicEvent>(publicEvent);

            try
            {
                findedPublicEvent.Feedbacks = feedbackService.GetFeedbacksFromData(findedPublicEvent.Id);
                return findedPublicEvent;
            }
            catch (Exception)
            {
                throw;
            }
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
            if (oldEvents == null)
            {
                throw new Exception();
            }
            var eventsToDelete = this.Mapper.Map<IList<PublicEvent>>(oldEvents);
            foreach (var oldEvent in eventsToDelete)
            {
                DeletePublicEvent(oldEvent.Id);
            }
        }
    }
}
