using AutoMapper;
using BGFolklore.Data.Models;
using BGFolklore.Models.Gallery.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<ImageViewModel, Image>().ReverseMap();
            this.CreateMap<Area, AreaImagesViewModel>();


            this.CreateMap<VideoViewModel, Video>().ReverseMap();
            this.CreateMap<Area, AreaVideosViewModel>();
        }
    }
}
