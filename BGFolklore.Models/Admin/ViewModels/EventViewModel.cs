using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Admin.ViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }

        public string OwnerId { get; set; }

        public string Name { get; set; }

        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public DateTime EventDateTime { get; set; }

        public int DurationInDays { get; set; }

        public int OccuringDays { get; set; }

        public PlaceType PlaceType { get; set; }

        public int IntendedFor { get; set; }

        public int TownId { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Description { get; set; }

        public float Rating { get; set; }

        public int StatusId { get; set; }

        public IList<Feedback> Feedbacks { get; set; }
    }
}
