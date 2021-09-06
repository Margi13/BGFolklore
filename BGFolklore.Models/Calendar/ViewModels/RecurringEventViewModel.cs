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
    public class RecurringEventViewModel : EventViewModel
    {
        public int OccuringDays { get; set; }

        public float Rating { get; set; }
    }
}
