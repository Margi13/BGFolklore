using BGFolklore.Common.Common;
using BGFolklore.Models.Calendar.BindingModels;

namespace BGFolklore.Models.Calendar.ViewModels
{
    public class FilterEventsViewModel
    {
        public FilterViewModel Filters { get; set; }
        public PaginatedList<UpcomingEventViewModel> UpcomingPaginatedList { get; set; }
        public PaginatedList<RecurringEventViewModel> RecurringPaginatedList { get; set; }

    }
}
