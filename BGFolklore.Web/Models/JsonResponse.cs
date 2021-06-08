using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Models
{
    public class JsonResponse
    {
        public IList<Area> EthnoAreas { get; set; }

    }
    public class Area
    {
        //public int Id { get; set; }
        [EnumDataType(typeof(AreasType))]
        public AreasType Type { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Area_Image { get; set; }
        public string Description_Images { get; set; }
        public string Description_Videos { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Video> Videos { get; set; }
    }
    public class Image
    {
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
