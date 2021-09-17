using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Admin.ViewModels;
using BGFolklore.Services.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Admin
{
    public class ManageEventsService : BaseService, IManageEventsService
    {
        public ManageEventsService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IList<ManageEventViewModel> GetAllEvents()
        {
            IList<ManageEventViewModel> eventViewModels = new List<ManageEventViewModel>();
            var eventsFromData = this.Context.PublicEvents.Where(pe=>pe.StatusId != (int)StatusName.Deleted);
            if (eventsFromData != null)
            {
                var publicEvents = this.Mapper.Map<IEnumerable<PublicEvent>>(eventsFromData).ToList();
                eventViewModels = this.Mapper.Map<IList<ManageEventViewModel>>(publicEvents);
                foreach (var viewModel in eventViewModels)
                {
                    AddEventFeedbacks(viewModel);
                    var userName = this.Context.Users.Where(u => u.Id.Equals(viewModel.OwnerId)).Select(u => u.UserName).FirstOrDefault().ToString();
                    viewModel.OwnerUserName = userName;
                }
            }
            return eventViewModels;
        }
        public ManageEventViewModel GetEventById(Guid eventId)
        {
            ManageEventViewModel eventViewModels = new ManageEventViewModel();
            var eventsFromData = this.Context.PublicEvents.Where(pe=>pe.Id.Equals(eventId)).FirstOrDefault();
            if (eventsFromData != null)
            {
                eventViewModels = this.Mapper.Map<ManageEventViewModel>(eventsFromData);

                    AddEventFeedbacks(eventViewModels);
                    var userName = this.Context.Users.Where(u => u.Id.Equals(eventViewModels.OwnerId)).Select(u => u.UserName).FirstOrDefault().ToString();
                eventViewModels.OwnerUserName = userName;

            }
            return eventViewModels;
        }
        public void AddEventFeedbacks(ManageEventViewModel eventViewModel)
        {
            var allEventFeedbacks = this.Context.Feedback.Where(f => f.EventId.Equals(eventViewModel.Id)).ToList();
            if (allEventFeedbacks != null)
            {
                eventViewModel.Feedbacks = this.Mapper.Map<IList<ManageFeedbackViewModel>>(allEventFeedbacks);
            }
            else
            {
                eventViewModel.Feedbacks = new List<ManageFeedbackViewModel>();
            }
        }
    }
}
