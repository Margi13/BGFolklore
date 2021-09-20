using BGFolklore.Data.Models;
using BGFolklore.Models.Admin.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Admin.Interfaces
{
    public interface IManageFeedbacksService
    {
        IList<ManageFeedbackViewModel> GetAllFeedbacks();
        ManageFeedbackViewModel GetFeedback(Guid feedId);
        void AddReportsToUser(User user);
        void AddEventFeedbacks(ManageEventViewModel eventViewModel);
        void AddDataToFeedbacks(IList<ManageFeedbackViewModel> reportViewModels);
    }
}
