using BGFolklore.Common.Nomenclatures;
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
    public interface IFilterService
    {
        IList<UpcomingEventViewModel> GetFilteredUpcomingEvents(FilterBindingModel filterBindingModel, IEnumerable<PublicEvent> upcomingEvents);
        IList<RecurringEventViewModel> GetFilteredRecurringEvents(FilterBindingModel filterBindingModel, IEnumerable<PublicEvent> recuringEvents);
        IEnumerable<PublicEvent> OrderFilteredData(Guid? ownerId, bool isRecurring, IEnumerable<PublicEvent> listToOrder);
    }
}