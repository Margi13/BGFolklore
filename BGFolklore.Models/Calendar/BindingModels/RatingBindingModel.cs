using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Calendar.BindingModels
{
    public class RatingBindingModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Rate { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public Guid EventId { get; set; }
    }
}
