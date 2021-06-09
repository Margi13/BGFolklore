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
        IList<AreaImagesViewModel> GetImagesFromJson(string jsonString);
    }
}
