using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Admin.ViewModels
{
    public class ManageFeedbackViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUserName { get; set; }

        public Guid EventId { get; set; }
        public string EventName { get; set; }

        public int StatusId { get; set; }

        public string Description { get; set; }
    }
}
