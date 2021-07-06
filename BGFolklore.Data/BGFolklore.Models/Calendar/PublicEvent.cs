using BGFolklore.Common.Nomenclatures;
using Microsoft.AspNetCore.Identity;
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
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

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
        public int TownId { get; set; }

        public Town Town { get; set; }


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
