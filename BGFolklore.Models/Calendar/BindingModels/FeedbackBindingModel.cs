using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Calendar.BindingModels
{
    public class FeedbackBindingModel
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreateDateTime { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        public int StatusId { get; set; }

        [Required(ErrorMessage = "Полето е задължително, ако искате да изпратите сигнал за грешна информация!")]
        [MaxLength(256, ErrorMessage ="Описанието трябва да е по-малко от 256 символа.")]
        [MinLength(3, ErrorMessage ="Описанието трябва да е от поне 3 символа.")]
        [Display(Name = "Опишете намерената грешна информация")]
        public string Description { get; set; }
    }
}
