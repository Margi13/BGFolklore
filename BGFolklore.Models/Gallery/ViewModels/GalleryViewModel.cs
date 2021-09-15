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
        public GalleryFilterViewModel FilterViewModel { get; set; }
        public IList<EthnoAreaViewModel> EthnoAreaViewModels { get; set; }
    }
}
