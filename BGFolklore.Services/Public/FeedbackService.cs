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

            var status = this.Context.Status.Where(s => s.Id == feedbackBindingModel.StatusId);
            newFeedback.Status = this.Mapper.Map<Status>(status.First());

            var pe = this.Context.PublicEvents.Where(pe => pe.Id == feedbackBindingModel.EventId);
            var publicEvent = this.Mapper.Map<PublicEvent>(pe.First());
            newFeedback.Event = publicEvent;
            
            var feedback = this.Mapper.Map<Feedback>(feedbackBindingModel);
            if(publicEvent.Feedbacks == null)
            {
                publicEvent.Feedbacks = new List<Feedback>();
            }
            publicEvent.Feedbacks.Add(feedback);

            var owner = this.Context.Users.Where(u => u.Id == feedbackBindingModel.OwnerId);
            newFeedback.Owner = this.Mapper.Map<User>(owner.First());

            this.Context.Feedback.Add(newFeedback);
            this.Context.SaveChanges();
        }
        public IList<Feedback> GetAllEventFeedbacks(Guid eventId)
        {
            var feedbacks = this.Context.Feedback.Where(f => f.EventId == eventId);
            IList<Feedback> feedsList = this.Mapper.Map<IList<Feedback>>(feedbacks);
            return feedsList;
        }

        public void ChangeFeedbackStatus(Guid feedbackId, int statusId)
        {
            var feedback = this.Context.Feedback.Where(f => f.Id == feedbackId);
            Feedback feedbackToChange = this.Mapper.Map<Feedback>(feedback);
            
            var newStatus = this.Context.Status.Where(s => s.Id == statusId);
            Status statusToChange = this.Mapper.Map<Status>(newStatus);
            
            feedbackToChange.Status = statusToChange;

            Context.SaveChanges();
        }

    }
}
