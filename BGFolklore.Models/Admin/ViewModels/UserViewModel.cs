using BGFolklore.Data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Admin.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string[] Roles { get; set; }

        public ICollection<PublicEvent> OwnPublicEvents { get; set; }

        public ICollection<Feedback> OwnReports { get; set; }
    }
}
