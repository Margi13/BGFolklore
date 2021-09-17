using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Admin.ViewModels
{
    public class ManageEventViewModel
    {
        public Guid Id { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUserName { get; set; }

        public string Name { get; set; }

        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Description { get; set; }

        public IList<ManageFeedbackViewModel> Feedbacks { get; set; }
    }
}
