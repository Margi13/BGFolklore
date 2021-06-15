using BGFolklore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Models.Gallery.ViewModels
{
    public class AreaVideosViewModel
    {
        [EnumDataType(typeof(AreasType))]
        public AreasType AreaType { get; set; }
        public string AreaName { get; set; }
        public ICollection<VideoViewModel> Videos { get; set; }
    }
}
