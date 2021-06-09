using BGFolklore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Gallery.ViewModels
{
    public class AreaImagesViewModel
    {
        [EnumDataType(typeof(AreasType))]
        public AreasType AreaType { get; set; }
        public string AreaName { get; set; }
        //public string Path { get; set; }
        public ICollection<Image> Images { get; set; }
        //public string Title { get; set; }
        //public string Description { get; set; }
    }
    public class Image
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
