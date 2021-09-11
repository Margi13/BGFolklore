using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Data.Models.Calendar
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public int Rate { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public Guid EventId { get; set; }
        public PublicEvent Event { get; set; }
    }
}
