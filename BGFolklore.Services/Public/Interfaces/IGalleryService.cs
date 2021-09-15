using BGFolklore.Data.Models.Gallery;
using BGFolklore.Models.Gallery.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public.Interfaces
{
    public interface IGalleryService
    {
        IEnumerable<EthnographicArea> GetEthnoAreasFromData();
        EthnographicArea GetEthnoAreaById(int areaId);
        IList<EthnoAreaViewModel> GetAllGalleryViewModels();
        IList<ImageViewModel> GetAllImages();
        IList<VideoViewModel> GetAllVideos();
        IList<ImageViewModel> GetFilteredImages(int costumeType);
        IList<VideoViewModel> GetFilteredVideos(string[] wordsToSearch);
    }
}
