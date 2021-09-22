using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Gallery.ViewModels
{
    public class GalleryViewModel
    {
        [Display(Name = "Търсене на")]
        public string WordsToSearch { get; set; }
        public IList<VideoViewModel> AllVideos { get; set; }
        public IList<EthnoAreaViewModel> EthnoAreaViewModels { get; set; }
    }
}
