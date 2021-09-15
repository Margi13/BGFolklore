using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Gallery.ViewModels
{
    public class VideoViewModel
    {
        [Required]
        [MaxLength(20)]
        public string AreaName { get; set; }
        [Required]
        public int EthnoAreaId { get; set; }

        [Required]
        public string YouTubeId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
