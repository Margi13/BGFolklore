using BGFolklore.Common.Nomenclatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BGFolklore.Data.Models.Calendar
{
    public class PublicEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime InsertDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        [Required]
        public DateTime EventDateTime { get; set; }

        public int OccuringDays { get; set; }

        [Required]
        public PlaceType PlaceType { get; set; }

        [Required]
        public int IntendedFor { get; set; }

        [Required]
        [MaxLength(60)]
        public string Town { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        public float Rating { get; set; }
    }
}
