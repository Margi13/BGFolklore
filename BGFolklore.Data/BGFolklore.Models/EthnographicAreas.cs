using BGFolklore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Data.Models
{
    public class EthnographicAreas
    {
        public IList<Area> EthnoAreas { get; set; }
    }
    public class Area
    {
        //public int Id { get; set; }
        [EnumDataType(typeof(AreasType))]
        public AreasType AreaType { get; set; }
        public string AreaName { get; set; }
        public string ImagesPath { get; set; }
        public string Area_Image { get; set; }
        public string Description_Images { get; set; }
        public string Description_Videos { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Video> Videos { get; set; }
    }

    public class Image
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Video
    {
        public string Id_YouTube { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
