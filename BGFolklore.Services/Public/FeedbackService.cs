using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data;
using BGFolklore.Data.Models;
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
    public class FeedbackService : BaseService, IFeedbackService
    {
        private readonly IStatusService statusService;

        public FeedbackService(ApplicationDbContext context, IMapper mapper, IStatusService statusService) : base(context, mapper)
        {
            this.statusService = statusService;
        }

        public Feedback GetFeedbackById(Guid feedbackId)
        {
            var feedback = this.Context.Feedback.Where(f => f.Id == feedbackId).FirstOrDefault();
            if (feedback == null)
            {
                throw new Exception();
            }
            feedback.Event = this.Context.PublicEvents.Where(pe => pe.Id.Equals(feedback.EventId)).FirstOrDefault();
            feedback.Owner = this.Context.Users.Where(u => u.Id.Equals(feedback.OwnerId)).FirstOrDefault();
            Feedback feedbackToReturn = this.Mapper.Map<Feedback>(feedback);
            return feedbackToReturn;
        }

        public void SaveFeedback(FeedbackBindingModel feedbackBindingModel)
        {
            feedbackBindingModel.StatusId = (int)StatusName.New;
            feedbackBindingModel.CreateDateTime = DateTime.Now;

            var newFeedback = this.Mapper.Map<Feedback>(feedbackBindingModel);

            var pe = this.Context.PublicEvents.Where(pe => pe.Id.Equals(feedbackBindingModel.EventId)).FirstOrDefault();
            var ow = this.Context.Users.Where(u => u.Id.Equals(feedbackBindingModel.OwnerId)).FirstOrDefault();

            try
            {
                PublicEvent publicEvent = this.Mapper.Map<PublicEvent>(pe);
                var owner = this.Mapper.Map<User>(ow);
                Status status = statusService.GetStatus(feedbackBindingModel.StatusId);

                if (publicEvent == null || owner == null || status == null)
                {
                    throw new Exception();
                }

                newFeedback.Event = publicEvent;
                newFeedback.Owner = owner;
                newFeedback.Status = status;

                this.Context.Feedback.Add(newFeedback);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public IList<Feedback> GetFeedbacksFromData(Guid eventId)
        {
            var feedbacks = this.Context.Feedback
                .Where(f => f.EventId == eventId && f.StatusId != 3)
                .OrderBy(f => f.StatusId)
                .ThenByDescending(f => f.CreateDateTime);
            if (feedbacks == null)
            {
                throw new Exception();
            }
            IList<Feedback> feedsList;
            try
            {
                feedsList = this.Mapper.Map<IList<Feedback>>(feedbacks);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return feedsList;
        }

        public IList<FeedbackViewModel> GetFeedbackViewModels(Guid eventId)
        {
            IList<FeedbackViewModel> feedbacks;
            try
            {
                IList<Feedback> feedsFromData = GetFeedbacksFromData(eventId);
                feedbacks = this.Mapper.Map<IList<FeedbackViewModel>>(feedsFromData);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return feedbacks;
        }

        public void ChangeFeedbackStatus(Guid feedbackId, int statusId)
        {
            try
            {
                Status newStatus = statusService.GetStatus(statusId);
                Feedback feedbackToChange = GetFeedbackById(feedbackId);

                feedbackToChange.StatusId = statusId;
                feedbackToChange.Status = newStatus;

                this.Context.Feedback.Update(feedbackToChange);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        public void DeleteAllEventFeedbacks(Guid eventId)
        {
            try
            {
                IList<Feedback> feedbacks = GetFeedbacksFromData(eventId);

                foreach (var feed in feedbacks)
                {
                    Status newStatus = statusService.GetStatus((int)StatusName.Deleted);

                    feed.StatusId = (int)StatusName.Deleted;
                    feed.Status = newStatus;
                }
                this.Context.Feedback.UpdateRange(feedbacks);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}
