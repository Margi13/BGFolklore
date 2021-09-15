using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Calendar.ViewModels
{
    public class OwnerEventsViewModel
    {
        public IList<RecurringEventViewModel> OwnerRecurringEventViewModels { get; set; }
        public IList<UpcomingEventViewModel> OwnerUpcomingEventViewModels { get; set; }
    }
}
