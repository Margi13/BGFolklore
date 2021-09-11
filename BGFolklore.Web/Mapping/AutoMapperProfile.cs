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

            this.CreateMap<PublicEvent, RecurringEventViewModel>();

            this.CreateMap<PublicEvent, EventViewModel>();

            this.CreateMap<AddEventBindingModel, PublicEvent>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Town, opt => opt.Ignore());

            this.CreateMap<PublicEvent, AddEventBindingModel>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore());

            this.CreateMap<AddEventBindingModel, AddEventViewModel>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore());

            this.CreateMap<EventViewModel, AddEventBindingModel>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore());

            this.CreateMap<FeedbackBindingModel, Feedback>()
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore());

            this.CreateMap<RatingBindingModel, Rating>()
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore());

            this.CreateMap<FeedbackBindingModel, FeedbackViewModel>();

            this.CreateMap<Feedback, FeedbackViewModel>();
        }
    }
}
