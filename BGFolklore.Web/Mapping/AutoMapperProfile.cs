using AutoMapper;
using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
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

            this.CreateMap<PublicEvent, UpcomingEventViewModel>();

            this.CreateMap<PublicEvent, RecurringEventViewModel > ();

            this.CreateMap<AddEventBindingModel,PublicEvent>()
                .ForMember(dest => dest.IntendedFor,opt=>opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt=>opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            this.CreateMap<AddEventBindingModel, AddEventViewModel>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore());
        }
    }
}
