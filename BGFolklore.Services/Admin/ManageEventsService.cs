using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data;
using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Admin.ViewModels;
using BGFolklore.Services.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BGFolklore.Services.Admin
{
    public class ManageEventsService : BaseService, IManageEventsService
    {
        private readonly IManageFeedbacksService manageFeedsService;

        public ManageEventsService(ApplicationDbContext context, IMapper mapper,
            IManageFeedbacksService manageFeedsService) : base(context, mapper)
        {
            this.manageFeedsService = manageFeedsService;
        }

        public IList<ManageEventViewModel> GetAllEvents()
        {
            IList<ManageEventViewModel> eventViewModels = new List<ManageEventViewModel>();
            try
            {
                var eventsFromData = this.Context.PublicEvents.Where(pe => pe.StatusId != (int)StatusName.Deleted);
                if (eventsFromData != null)
                {
                    var publicEvents = this.Mapper.Map<IEnumerable<PublicEvent>>(eventsFromData).ToList();
                    eventViewModels = this.Mapper.Map<IList<ManageEventViewModel>>(publicEvents);
                    foreach (var viewModel in eventViewModels)
                    {
                        manageFeedsService.AddEventFeedbacks(viewModel);
                    }
                    AddDataToEvent(eventViewModels);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return eventViewModels;
        }
        public ManageEventViewModel GetEvent(Guid eventId)
        {
            ManageEventViewModel eventViewModel = new ManageEventViewModel();
            var eventFromData = this.Context.PublicEvents.Where(pe => pe.Id.Equals(eventId)).FirstOrDefault();
            if (eventFromData != null)
            {
                eventViewModel = this.Mapper.Map<ManageEventViewModel>(eventFromData);

                manageFeedsService.AddEventFeedbacks(eventViewModel);
                var userName = this.Context.Users.Where(u => u.Id.Equals(eventViewModel.OwnerId)).Select(u => u.UserName).FirstOrDefault().ToString();
                eventViewModel.OwnerUserName = userName;

            }
            return eventViewModel;
        }

        public void AddEventsToUser(User user)
        {
            try
            {
                var allUserEventsFromData = this.Context.PublicEvents.Where(pe => pe.OwnerId == user.Id).ToList();
                if (allUserEventsFromData != null)
                {
                    user.PublicEvents = this.Mapper.Map<IList<PublicEvent>>(allUserEventsFromData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AddDataToEvent(IList<ManageEventViewModel> eventViewModels)
        {
            foreach (var viewModel in eventViewModels)
            {
                AddOwnerName(viewModel);
            }
        }
        private void AddOwnerName(ManageEventViewModel viewModel)
        {
                var userName = this.Context.Users.Where(e => e.Id.Equals(viewModel.OwnerId)).Select(u => u.UserName).FirstOrDefault();
                if (userName != null)
                {
                    viewModel.OwnerUserName = userName;
                }
                else
                {
                    viewModel.OwnerUserName = "Не е намерен!";
                }
        }

    }
}
