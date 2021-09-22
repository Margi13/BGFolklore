using AutoMapper;
using BGFolklore.Data;
using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Gallery;
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
        //public static IEnumerable<EthnographicArea> EthnoAreas { get; set; }
        public GalleryService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<EthnographicArea> GetEthnoAreasFromData()
        {
            IEnumerable<EthnographicArea> ethnoAreas;
            try
            {
                var ethnoAreaFromData = this.Context.EthnographicAreas;

                if (ethnoAreaFromData == null)
                {
                    return null;
                }
                ethnoAreas = this.Mapper.Map<IEnumerable<EthnographicArea>>(ethnoAreaFromData);
            }
            catch (Exception)
            {
                throw;
            }
            return ethnoAreas;
        }
        public EthnographicArea GetEthnoAreaById(int areaId)
        {
            EthnographicArea ethnoAreas;
            try
            {
                var ethnoAreaFromData = this.Context.EthnographicAreas.Where(a => a.Id == areaId).FirstOrDefault();

                if (ethnoAreaFromData == null)
                {
                    return null;
                }
                ethnoAreas = this.Mapper.Map<EthnographicArea>(ethnoAreaFromData);
            }
            catch (Exception)
            {
                throw;
            }
            return ethnoAreas;
        }

        public IList<EthnoAreaViewModel> GetAllGalleryViewModels()
        {
            IList<EthnoAreaViewModel> ethnoAreaViewModels = new List<EthnoAreaViewModel>();

            var ethnoAreas = GetEthnoAreasFromData();

            foreach (var ethnoArea in ethnoAreas)
            {
                var viewModel = this.Mapper.Map<EthnoAreaViewModel>(ethnoArea);
                viewModel.ImageViewModels = GetEthnoAreaImages(ethnoArea.Id);
                viewModel.VideoViewModels = GetEthnoAreaVideos(ethnoArea.Id);
                ethnoAreaViewModels.Add(viewModel);
            }
            
            return ethnoAreaViewModels.ToList();
        }

        public IList<ImageViewModel> GetAllImages()
        {
            IList<ImageViewModel> images;
            try
            {
                var imagesFromData = this.Context.Images;

                if (imagesFromData == null)
                {
                    return null;
                }
                images = this.Mapper.Map<IList<ImageViewModel>>(imagesFromData);
                foreach (var img in images)
                {
                    var ethnoArea = GetEthnoAreaById(img.EthnoAreaId);
                    img.AreaName = ethnoArea.AreaName;
                    img.ImagesPath = Path.Combine(ethnoArea.ImagesPath, img.FileName);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return images;
        }
        public IList<VideoViewModel> GetAllVideos()
        {
            IList<VideoViewModel> videos;
            try
            {
                var videosFromData = this.Context.Videos;

                if (videosFromData == null)
                {
                    return null;
                }
                videos = this.Mapper.Map<IList<VideoViewModel>>(videosFromData);
                foreach (var vid in videos)
                {
                    var ethnoArea = GetEthnoAreaById(vid.EthnoAreaId);
                    vid.AreaName = ethnoArea.AreaName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return videos;
        }
        private IList<ImageViewModel> GetEthnoAreaImages(int areaId)
        {
            IList<ImageViewModel> images;
            try
            {
                var imagesFromData = this.Context.Images.Where(img => img.EthnoAreaId == areaId);

                if (imagesFromData == null)
                {
                    return null;
                }
                imagesFromData = imagesFromData.OrderBy(i => i.Title);
                images = this.Mapper.Map<IList<ImageViewModel>>(imagesFromData);
                foreach (var img in images)
                {
                    var ethnoArea = GetEthnoAreaById(img.EthnoAreaId);
                    img.AreaName = ethnoArea.AreaName;
                    img.ImagesPath = Path.Combine(ethnoArea.ImagesPath, img.FileName);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return images;
        }

        private IList<VideoViewModel> GetEthnoAreaVideos(int areaId)
        {
            IList<VideoViewModel> videos;
            try
            {
                var videosFromData = this.Context.Videos.Where(vid => vid.EthnoAreaId == areaId);

                if (videosFromData == null)
                {
                    return null;
                }
                videosFromData = videosFromData.OrderBy(v => v.Title);
                videos = this.Mapper.Map<IList<VideoViewModel>>(videosFromData);
                foreach (var vid in videos)
                {
                    var ethnoArea = GetEthnoAreaById(vid.EthnoAreaId);
                    vid.AreaName = ethnoArea.AreaName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return videos;
        }

        public IList<ImageViewModel> GetFilteredImages(int costumeType)
        {
            IList<ImageViewModel> images;
            try
            {
                var imagesFromData = this.Context.Images.Where(img => img.CostumeType == costumeType);

                if (imagesFromData == null)
                {
                    return null;
                }
                images = this.Mapper.Map<IList<ImageViewModel>>(imagesFromData);
                foreach (var img in images)
                {
                }
            }
            catch (Exception)
            {
                throw;
            }
            return images;
        }
        public IList<VideoViewModel> GetFilteredVideos(string[] wordsToSearch)
        {
            IList<VideoViewModel> videos = new List<VideoViewModel>();
            try
            {
                var videosFromData = this.Context.Videos;
                if (videosFromData == null)
                {
                    return null;
                }
                foreach (var word in wordsToSearch)
                {
                    var findedVideos = videosFromData.Where(vid => vid.Title.Contains(word) || vid.Description.Contains(word));
                    if (findedVideos != null)
                    {
                        var resultedVideos = this.Mapper.Map<IList<VideoViewModel>>(findedVideos);
                        foreach (var vid in resultedVideos)
                        {
                            var ethnoArea = GetEthnoAreaById(vid.EthnoAreaId);
                            vid.AreaName = ethnoArea.AreaName;
                            videos.Add(vid);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return videos;
        }
    }
}
