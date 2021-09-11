﻿using AutoMapper;
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

        public IList<Feedback> GetFeedbacksFromData(Guid eventId)
        {
            var feedbacks = this.Context.Feedback
                .Where(f => f.EventId == eventId && f.StatusId != 3)
                .OrderBy(f=>f.StatusId)
                .ThenByDescending(f=>f.CreateDateTime);
            IList<Feedback> feedsList = this.Mapper.Map<IList<Feedback>>(feedbacks);
            return feedsList;
        }
        public IList<FeedbackViewModel> GetFeedbackViewModels(Guid eventId)
        {
            IList<Feedback> feedsFromData = GetFeedbacksFromData(eventId);
            IList<FeedbackViewModel> feedbacks = this.Mapper.Map<IList<FeedbackViewModel>>(feedsFromData);
            return feedbacks;
        }
        public void DeleteAllEventFeedbacks(Guid eventId)
        {
            IList<Feedback> feedbacks = GetFeedbacksFromData(eventId);

            foreach (var feed in feedbacks)
            {
                Status newStatus = GetStatus((int)StatusName.Deleted);

                feed.StatusId = (int)StatusName.Deleted;
                feed.Status = newStatus;
                this.Context.Feedback.Update(feed);
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
