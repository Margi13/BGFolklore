using BGFolklore.Common.Nomenclatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Calendar.BindingModels
{
    public class FilterBindingModel
    {
        public Guid TownId { get; set; }
        public Guid AreaId { get; set; }
        public Guid OwnerId { get; set; }
        public PlaceType PlaceType { get; set; }

        public DateTime BeforeDate { get; set; }
        public DateTime AfterDate { get; set; }
        public int DurationInDays { get; set; }
        public int OccuringDays { get; set; }
        public int IntendedFor { get; set; }
    }
}
