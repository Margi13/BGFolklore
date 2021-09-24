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
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Admin
{
    public class ManageFeedbacksService : BaseService, IManageFeedbacksService
    {
        public ManageFeedbacksService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IList<ManageFeedbackViewModel> GetAllFeedbacks()
        {
            IList<ManageFeedbackViewModel> feedbacksViewModel = new List<ManageFeedbackViewModel>();
            try
            {
                var feedbacksFromData = this.Context.Feedback.Where(f => f.StatusId != (int)StatusName.Deleted);
                if (feedbacksFromData != null)
                {
                    var feeds = this.Mapper.Map<IEnumerable<Feedback>>(feedbacksFromData);
                    feedbacksViewModel = this.Mapper.Map<IList<ManageFeedbackViewModel>>(feeds);
                    AddDataToFeedbacks(feedbacksViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return feedbacksViewModel;

        }
        public ManageFeedbackViewModel GetFeedback(Guid feedId)
        {
            ManageFeedbackViewModel feedbackViewModel = new ManageFeedbackViewModel();
            var feedbackFromData = this.Context.Feedback.Where(f => f.Id.Equals(feedId)).FirstOrDefault();
            if (feedbackFromData != null)
            {
                feedbackViewModel = this.Mapper.Map<ManageFeedbackViewModel>(feedbackFromData);
                AddEventName(feedbackViewModel);
                AddOwnerName(feedbackViewModel);
            }
            return feedbackViewModel;
        }

        public void AddEventFeedbacks(ManageEventViewModel eventViewModel)
        {
            try
            {
                IList<Feedback> allEventFeedbacks = this.Context.Feedback.Where(f => f.EventId.Equals(eventViewModel.Id) && f.StatusId != (int)StatusName.Deleted).ToList();
                if (allEventFeedbacks != null)
                {
                    eventViewModel.Feedbacks = this.Mapper.Map<IList<ManageFeedbackViewModel>>(allEventFeedbacks);
                    AddDataToFeedbacks(eventViewModel.Feedbacks);
                }
            }
            catch (Exception)
            {
                eventViewModel.Feedbacks = new List<ManageFeedbackViewModel>();
            }
        }
        public void AddReportsToUser(User user)
        {
            try
            {
                var allUserReportsFromData = this.Context.Feedback.Where(f => f.OwnerId == user.Id).ToList();
                if (allUserReportsFromData != null)
                {
                    user.Reports = this.Mapper.Map<IList<Feedback>>(allUserReportsFromData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AddDataToFeedbacks(IList<ManageFeedbackViewModel> reportViewModels)
        {
            foreach (var viewModel in reportViewModels)
            {
                AddOwnerName(viewModel);
                AddEventName(viewModel);
            }
        }
        private void AddOwnerName(ManageFeedbackViewModel viewModel)
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
        private void AddEventName(ManageFeedbackViewModel viewModel)
        {
            var eventName = this.Context.PublicEvents.Where(e => e.Id.Equals(viewModel.EventId)).Select(e => e.Name).FirstOrDefault();
            if (eventName != null)
            {
                viewModel.EventName = eventName;
            }
            else
            {
                viewModel.EventName = "Не е намеренo!";
            }
        }
    }
}
