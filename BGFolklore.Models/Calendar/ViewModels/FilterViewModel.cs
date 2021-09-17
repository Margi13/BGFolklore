using BGFolklore.Common.Nomenclatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Calendar.ViewModels
{
    public class FilterViewModel
    {
        public string OwnerId { get; set; }
        [Required]
        public bool IsRecurring { get; set; }

        [Display(Name = "Област")]
        public int AreaId { get; set; }

        [Display(Name = "Град")]
        public int TownId { get; set; }

        [Display(Name = "Тип на мястото")]
        public PlaceType? PlaceType { get; set; }

        [Display(Name = "След дата")]
        public DateTime? AfterDate { get; set; }

        [Display(Name = "Преди дата")]
        public DateTime? BeforeDate { get; set; }

        [Display(Name = "Препоръчано за")]
        public List<SelectListItem> IntendedFor { get; set; }

        [Display(Name = "Продължителност в дни")]
        public int DurationInDays { get; set; }

        [Display(Name = "Дни от седмицата")]
        public List<SelectListItem> OccuringDays { get; set; }
    }
}
