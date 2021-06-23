using AutoMapper;
using BGFolklore.Data;
using BGFolklore.Data.Models;
using BGFolklore.Models.Gallery.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public
{
    public class GalleryService : BaseService, IGalleryService
    {
        public GalleryService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IList<AreaImagesViewModel> GetImagesFromJson(string jsonString)
        {
            //try catch?
            EthnographicAreas areas = JsonConvert.DeserializeObject<EthnographicAreas>(jsonString);
            foreach (var area in areas.EthnoAreas)
            {
                foreach (var image in area.Images)
                {
                    image.Path = Path.Combine(area.ImagesPath, image.FileName);
                }
            }
            IList<AreaImagesViewModel> areaImages = this.Mapper.Map<List<AreaImagesViewModel>>(areas.EthnoAreas); //

            return areaImages;
        }

        public IList<AreaVideosViewModel> GetVideosFromJson(string jsonString)
        {
            EthnographicAreas areas = JsonConvert.DeserializeObject<EthnographicAreas>(jsonString);
            IList<AreaVideosViewModel> areaVideos = this.Mapper.Map<List<AreaVideosViewModel>>(areas.EthnoAreas); //

            return areaVideos;
        }
    }
}
