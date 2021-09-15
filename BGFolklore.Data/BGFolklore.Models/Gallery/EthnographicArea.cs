using BGFolklore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Data.Models.Gallery
{
    public class EthnographicArea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string AreaName { get; set; }

        [Required]
        public string MapImageFileName { get; set; }
        
        [Required]
        public string ImagesPath { get; set; }

    }
}
