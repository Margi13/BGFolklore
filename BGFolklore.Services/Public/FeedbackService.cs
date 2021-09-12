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
        public FeedbackService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public void SaveFeedback(FeedbackBindingModel feedbackBindingModel)
        {
            feedbackBindingModel.StatusId = (int)StatusName.New;
            feedbackBindingModel.CreateDateTime = DateTime.Now;

            var newFeedback = this.Mapper.Map<Feedback>(feedbackBindingModel);

            var pe = this.Context.PublicEvents.Where(pe => pe.Id == feedbackBindingModel.EventId).FirstOrDefault();
            var ow = this.Context.Users.Where(u => u.Id == feedbackBindingModel.OwnerId).FirstOrDefault();

            try
            {
                PublicEvent publicEvent = this.Mapper.Map<PublicEvent>(pe);
                var owner = this.Mapper.Map<User>(ow);
                Status status = GetStatus(feedbackBindingModel.StatusId);

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

                throw;
            }
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
                throw;
            }
            return feedbacks;
        }
        public void DeleteAllEventFeedbacks(Guid eventId)
        {
            try
            {
                IList<Feedback> feedbacks = GetFeedbacksFromData(eventId);

                foreach (var feed in feedbacks)
                {
                    Status newStatus = GetStatus((int)StatusName.Deleted);

                    feed.StatusId = (int)StatusName.Deleted;
                    feed.Status = newStatus;
                }
                this.Context.Feedback.UpdateRange(feedbacks);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
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
            IList<Feedback> feedsList = this.Mapper.Map<IList<Feedback>>(feedbacks);
            return feedsList;
        }
        public void ChangeFeedbackStatus(Guid feedbackId, int statusId)
        {
            try
            {
                Feedback feedbackToChange = GetFeedbackById(feedbackId);
                Status newStatus = GetStatus(statusId);
                feedbackToChange.StatusId = statusId;
                feedbackToChange.Status = newStatus;
                this.Context.Feedback.Update(feedbackToChange);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public Feedback GetFeedbackById(Guid feedbackId)
        {
            var feedback = this.Context.Feedback.Where(f => f.Id == feedbackId).FirstOrDefault();
            if (feedback == null)
            {
                throw new Exception();
            }
            Feedback feedbackToReturn = this.Mapper.Map<Feedback>(feedback);
            return feedbackToReturn;
        }

        private Status GetStatus(int statusId)
        {
            var newStatus = this.Context.Status.Where(s => s.Id == statusId).FirstOrDefault();
            if (newStatus == null)
            {
                throw new Exception();
            }
            Status status = this.Mapper.Map<Status>(newStatus);
            return status;
        }
    }
}
