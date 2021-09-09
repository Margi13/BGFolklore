using AutoMapper;
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
            feedbackBindingModel.StatusId = 1;
            feedbackBindingModel.CreateDateTime = DateTime.Now;

            var newFeedback = this.Mapper.Map<Feedback>(feedbackBindingModel);

            var pe = this.Context.PublicEvents.Where(pe => pe.Id == feedbackBindingModel.EventId);
            PublicEvent publicEvent = this.Mapper.Map<PublicEvent>(pe.First());
            newFeedback.Event = publicEvent;

            var owner = this.Context.Users.Where(u => u.Id == feedbackBindingModel.OwnerId);
            newFeedback.Owner = this.Mapper.Map<User>(owner.First());

            Status status = GetStatus(feedbackBindingModel.StatusId);
            newFeedback.Status = status;

            this.Context.Feedback.Add(newFeedback);
            this.Context.SaveChanges();
        }

        public IList<Feedback> GetAllEventFeedbacks(Guid eventId)
        {
            var feedbacks = this.Context.Feedback.Where(f => f.EventId == eventId && f.StatusId != 3);
            IList<Feedback> feedsList = this.Mapper.Map<IList<Feedback>>(feedbacks);
            return feedsList;
        }
        public void DeleteAllEventFeedbacks(Guid eventId)
        {
            var feedbacks = GetAllEventFeedbacks(eventId);

            foreach (var feed in feedbacks)
            {
                Status newStatus = GetStatus(3);

                feed.StatusId = 3;
                feed.Status = newStatus;
            }
            Context.SaveChanges();
        }
        public void ChangeFeedbackStatus(Guid feedbackId, int statusId)
        {
            Feedback feedbackToChange = GetFeedbackById(feedbackId);
            Status newStatus = GetStatus(statusId);
            
            feedbackToChange.StatusId = statusId;
            feedbackToChange.Status = newStatus;

            Context.SaveChanges();
        }
        public Feedback GetFeedbackById(Guid feedbackId) 
        {
            var feedback = this.Context.Feedback.Where(f => f.Id == feedbackId);
            Feedback feedbackToReturn = this.Mapper.Map<Feedback>(feedback.First());
            return feedbackToReturn;
        }

        private Status GetStatus(int statusId)
        {
            var newStatus = this.Context.Status.Where(s => s.Id == statusId);
            Status status = this.Mapper.Map<Status>(newStatus.First());

            return status;
        }
    }
}
