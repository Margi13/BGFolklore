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
            return recurringEvents;
        }

        public IList<UpcomingEventViewModel> GetUpcomingEvents()
        {
            var publicEvents = this.Context.PublicEvents.Where(e => e.OccuringDays == 0).Where(e => e.StatusId != 3);
            IList<UpcomingEventViewModel> upcomingEvents = this.Mapper.Map<IList<UpcomingEventViewModel>>(publicEvents);
            return upcomingEvents;
        }

        public void SaveAddEvent(AddEventBindingModel newEvent)
        {
            var newPublicEvent = this.Mapper.Map<PublicEvent>(newEvent);
            newPublicEvent.InsertDateTime = DateTime.Now;

            foreach (int attendeeType in newEvent.IntendedFor)
            {
                newPublicEvent.IntendedFor = newPublicEvent.IntendedFor | attendeeType;
            }

            if (newEvent.IsRecurring == true)
            {
                foreach (int dayName in newEvent.OccuringDays)
                {
                    newPublicEvent.OccuringDays = newPublicEvent.OccuringDays | dayName;
                }
            }

            newPublicEvent.Town = townsService.GetTownByGivenId(newEvent.TownId);

            var status = Context.Status.Where(s => s.Id == 1);
            Status newStatus = this.Mapper.Map<Status>(status.First());
            newPublicEvent.Status = newStatus;

            newPublicEvent.Feedbacks = new List<Feedback>();

            this.Context.PublicEvents.Add(newPublicEvent);
            this.Context.SaveChanges();
        }
        public PublicEvent GetPublicEvent(Guid eventId)
        {
            var publicEvent = this.Context.PublicEvents.Where(pe => pe.Id == eventId).First();
            PublicEvent findedPublicEvent = this.Mapper.Map<PublicEvent>(publicEvent);
            return findedPublicEvent;
        }


        public void UpdatePublicEvent(EventViewModel eventViewModel, PublicEvent updatedPublicEvent)
        {
            PublicEvent publicEvent = GetPublicEvent(eventViewModel.Id);

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
    }
}
