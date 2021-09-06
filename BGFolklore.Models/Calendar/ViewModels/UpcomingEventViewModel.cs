using BGFolklore.Common;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Calendar.ViewModels
{
    public class UpcomingEventViewModel : EventViewModel
    {
        public int DurationInDays { get; set; }
    }
}
