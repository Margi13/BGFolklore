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
        IList<UpcomingEventViewModel> GetUpcomingEvents();
        IList<RecurringEventViewModel> GetRecurringEvents();
        public EventViewModel GetEventViewModel(Guid eventId);
        void SaveAddEvent(AddEventBindingModel newEvent);
        void UpdatePublicEvent(Guid eventId, AddEventBindingModel updatedViewModel);
        void DeletePublicEvent(Guid eventId);

        AddEventBindingModel GetBindingModelFromData(Guid eventId);
    }
}
