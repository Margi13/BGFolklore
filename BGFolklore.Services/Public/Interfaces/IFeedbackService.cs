using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
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
        IList<FeedbackViewModel> GetFeedbackViewModels(Guid eventId);
        IList<Feedback> GetFeedbacksFromData(Guid eventId);
        void ChangeFeedbackStatus(Guid feedbackId, int statusId);
        void DeleteAllEventFeedbacks(Guid eventId);
        Feedback GetFeedbackById(Guid feedbackId);

    }
}
