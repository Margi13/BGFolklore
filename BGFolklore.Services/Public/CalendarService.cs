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
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays != 0).Where(e => e.StatusId != 3);
            IList<RecurringEventViewModel> recurringEvents = this.Mapper.Map<IList<RecurringEventViewModel>>(publicEvents);
            //foreach (var recurringEvent in recurringEvents)
            //{
            //    recurringEvent.Feedbacks = feedbackService.GetAllEventFeedbacks(recurringEvent.Id);
            //}
            return recurringEvents;
        }

        public IList<UpcomingEventViewModel> GetUpcomingEvents()
        {
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays == 0).Where(e => e.StatusId != 3);
            IList<UpcomingEventViewModel> upcomingEvents = this.Mapper.Map<IList<UpcomingEventViewModel>>(publicEvents);
            //foreach (var upcomingEvent in upcomingEvents)
            //{
            //    upcomingEvent.Feedbacks = feedbackService.GetAllEventFeedbacks(upcomingEvent.Id);
            //}
            return upcomingEvents;
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

        public void DeletePublicEvent(EventViewModel eventViewModel)
        {
            PublicEvent publicEvent = GetPublicEvent(eventViewModel.Id);

            publicEvent.StatusId = 3;

            var status = Context.Status.Where(s => s.Id == 3);
            Status newStatus = this.Mapper.Map<Status>(status.First());

            publicEvent.Status = newStatus;

            this.Context.SaveChanges();
        }
        private PublicEvent GetPublicEvent(Guid eventId)
        {
            var publicEvent = this.Context.PublicEvents.Where(pe => pe.Id == eventId).First();
            PublicEvent findedPublicEvent = this.Mapper.Map<PublicEvent>(publicEvent);
            findedPublicEvent.Feedbacks = feedbackService.GetAllEventFeedbacks(findedPublicEvent.Id);

            return findedPublicEvent;
        }
        public AddEventBindingModel GetBindingModelFromData(EventViewModel eventViewModel)
        {
            PublicEvent publicEvent = GetPublicEvent(eventViewModel.Id);
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
    }
}
