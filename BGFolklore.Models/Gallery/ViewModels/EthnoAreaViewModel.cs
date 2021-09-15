using BGFolklore.Data.Models.Gallery;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Gallery.ViewModels
{
    public class EthnoAreaViewModel
    {
        [Required]
        [MaxLength(20)]
        public string AreaName { get; set; }

        [Required]
        public string MapImageFileName { get; set; }

        [Required]
        [MaxLength(256)]
        public string ImagesDescription { get; set; }

        [Required]
        [MaxLength(256)]
        public string VideosDescription { get; set; }

        public IEnumerable<ImageViewModel> ImageViewModels { get; set; }
        public IEnumerable<VideoViewModel> VideoViewModels { get; set; }
    }
}
