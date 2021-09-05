using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public.Interfaces
{
    public interface IFeedbackService
    {
        void SaveFeedback(FeedbackBindingModel feedbackBindingModel);
        IList<Feedback> GetAllEventFeedbacks(Guid id);
        void ChangeFeedbackStatus(Guid feedbackId, int statusId);
    }
}
