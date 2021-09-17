using AutoMapper;
using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Data.Models.Gallery;
using BGFolklore.Models.Admin.ViewModels;
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
            //Image Maps
            this.CreateMap<Image, ImageViewModel>();
            this.CreateMap<EthnoAreaViewModel, ImageViewModel>();
            this.CreateMap<Video, VideoViewModel>();
            this.CreateMap<EthnoAreaViewModel, VideoViewModel>();
            this.CreateMap<EthnographicArea, EthnoAreaViewModel>();

            //PublicEvent Maps
            this.CreateMap<PublicEvent, UpcomingEventViewModel>();
            this.CreateMap<PublicEvent, RecurringEventViewModel>();
            this.CreateMap<PublicEvent, EventViewModel>();
            this.CreateMap<PublicEvent, AddEventBindingModel>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore());

            //ViewModel Maps
            this.CreateMap<AddEventBindingModel, PublicEvent>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Town, opt => opt.Ignore());

            this.CreateMap<AddEventBindingModel, AddEventViewModel>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore());

            this.CreateMap<EventViewModel, AddEventBindingModel>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore());

            this.CreateMap<FilterBindingModel, FilterViewModel>()
                .ForMember(dest => dest.IntendedFor, opt => opt.Ignore())
                .ForMember(dest => dest.OccuringDays, opt => opt.Ignore());

            //Feedback Maps
            this.CreateMap<FeedbackBindingModel, Feedback>()
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore());

            this.CreateMap<FeedbackBindingModel, FeedbackViewModel>();

            this.CreateMap<Feedback, FeedbackViewModel>();

            //Rating Maps
            this.CreateMap<RatingBindingModel, Rating>()
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore());

            //Admin managment maps
            this.CreateMap<User, ManageUserViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.ActivePublicEvents, opt => opt.Ignore())
                .ForMember(dest => dest.ActiveReports, opt => opt.Ignore())
                .ForMember(dest => dest.AllEventsCount, opt => opt.MapFrom(s => s.PublicEvents.Count()))
                .ForMember(dest => dest.AllReportsCount, opt => opt.MapFrom(s => s.Reports.Count()));

            this.CreateMap<PublicEvent, ManageEventViewModel>();

            this.CreateMap<Feedback, ManageFeedbackViewModel>();
        }
    }
}
