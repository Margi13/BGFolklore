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
    public interface ICalendarService
    {
        OwnerEventsViewModel GetAllEventsForUser(string userId);
        IList<UpcomingEventViewModel> GetUpcomingEvents(FilterBindingModel filterBindingModel);
        IList<RecurringEventViewModel> GetRecurringEvents(FilterBindingModel filterBindingModel);
        public EventViewModel GetEventViewModel(Guid eventId);
        void SaveAddEvent(AddEventBindingModel newEvent);
        void UpdatePublicEvent(Guid eventId, AddEventBindingModel updatedViewModel);
        void DeletePublicEvent(Guid eventId);

        AddEventBindingModel GetBindingModelFromData(Guid eventId);
    }
}
