using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Calendar.BindingModels
{
    public class AddEventBindingModel
    {
        [Required]
        public string OwnerId { get; set; }

        [Required(ErrorMessage = "Не сте посочили дата/час!")]
        [DataType(DataType.DateTime, ErrorMessage = "Въведете валидни дата и час [dd/mm/yyyy hh:mm]")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0:dd/MM/yyyy hh:mm")]
        public DateTime EventDateTime { get; set; }

        [Required(ErrorMessage = "Не сте посочили име!")]
        [MaxLength(256)]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Ще се повтаря ли всяка седмица?")]
        public bool IsRecurring { get; set; }

        //[Required]
        public int DurationInDays { get; set; }

        //[Required(ErrorMessage = "Изберете поне една от опциите!")]
        public int[] OccuringDays { get; set; }

        [Required(ErrorMessage = "Моля изберете една от опциите!")]
        public PlaceType PlaceType { get; set; }

        [Required(ErrorMessage = "Изберете поне една от опциите!")]
        public int[] IntendedFor { get; set; }

        [Required(ErrorMessage = "Не сте посочили град!")]
        public int TownId { get; set; }

        [Required(ErrorMessage = "Не сте посочили адрес!")]
        [MaxLength(250)]
        [MinLength(6, ErrorMessage = "Адресът не може да е по-малко от 6 символа!")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Не сте посочили телефон за връзка!")]
        [MaxLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не сте посочили описание на събитието!")]
        [MaxLength(250)]
        [MinLength(10, ErrorMessage = "Адресът не може да е по-малко от 10 символа!")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}
