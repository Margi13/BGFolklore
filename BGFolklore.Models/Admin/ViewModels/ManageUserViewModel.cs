using BGFolklore.Data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Admin.ViewModels
{
    public class ManageUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public IList<string> Roles { get; set; }

        public IList<PublicEvent> ActivePublicEvents { get; set; }
        public int AllEventsCount { get; set; }
                
        public IList<Feedback> ActiveReports { get; set; }
        public int AllReportsCount { get;set; }
    }
}
