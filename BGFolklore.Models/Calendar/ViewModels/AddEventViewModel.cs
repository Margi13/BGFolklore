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
    public class AddEventViewModel
    {
        [Required]
        public string OwnerId { get; set; }

        [Required(ErrorMessage = "Не сте посочили дата/час!")]
        [Display(Name = "Кога започва събитието?")]
        [DataType(DataType.DateTime, ErrorMessage = "Въведете валидни дата и час [dd/mm/yyyy hh:mm]")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0:dd/mm/yyyy hh:mm")]
        public DateTime? EventDateTime { get; set; }

        [Required(ErrorMessage = "Не сте посочили име!")]
        [MaxLength(256, ErrorMessage = "Името не може да е повече от 256 символа!")]
        [Display(Name = "Име на събитието")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Ще се повтаря ли всяка седмица?")]
        public bool IsRecurring { get; set; }

        //[Required]
        [Display(Name = "Продължителност в дни")]
        public int DurationInDays { get; set; }

        [Required(ErrorMessage = "Моля изберете поне една от опциите!")]
        [Display(Name = "В кои дни ще се провежда?")]
        public List<SelectListItem> OccuringDays { get; set; }

        [Required(ErrorMessage = "Моля изберете една от опциите!")]
        [Display(Name = "Какво представлява мястото на събитието?")]
        public PlaceType PlaceType { get; set; }

        [Required(ErrorMessage = "Моля изберете поне една от опциите!")]
        [Display(Name = "За кого е подходящо?")]
        public List<SelectListItem> IntendedFor { get; set; }

        [Required(ErrorMessage = "Не сте посочили град!")]
        [Display(Name = "Град")]
        public int TownId { get; set; }

        [Required(ErrorMessage = "Не сте посочили адрес!")]
        [MaxLength(250, ErrorMessage = "Адресът не може да е повече от 250 символа!")]
        [MinLength(6, ErrorMessage = "Адресът не може да е по-малко от 6 символа!")]
        [Display(Name = "Адрес")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Не сте посочили телефон за връзка!")]
        [MaxLength(20, ErrorMessage = "Телефонът не може да е повече от 20 символа!")]
        [Display(Name = "Телефон за връзка")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не сте посочили описание на събитието!")]
        [MaxLength(250, ErrorMessage = "Описанието не може да е повече от 250 символа!")]
        [MinLength(10, ErrorMessage = "Описанието не може да е по-малко от 10 символа!")]
        [Display(Name = "Описание на събитието")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
