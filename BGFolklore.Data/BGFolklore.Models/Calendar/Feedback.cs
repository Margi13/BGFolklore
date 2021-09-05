using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Data.Models.Calendar
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreateDateTime { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public Guid EventId { get; set; }
        public PublicEvent Event { get; set; }

        [Required]
        public int StatusId { get; set; }
        public Status Status { get; set; }

        [Required]
        [MaxLength(256)]
        public string Description { get; set; }
    }
}
