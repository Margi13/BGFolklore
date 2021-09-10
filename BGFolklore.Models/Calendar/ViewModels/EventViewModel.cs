using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BGFolklore.Models.Calendar.ViewModels
{
    public class EventViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        [Required]
        public DateTime EventDateTime { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public PlaceType PlaceType { get; set; }

        [Required]
        public int IntendedFor { get; set; }

        [Required]
        public int TownId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [JsonIgnore]
        public IList<FeedbackViewModel> Feedbacks { get; set; }
    }
}
