using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using BGFolklore.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Common
{
    public class ViewModelHelper
    {
        private readonly IStringLocalizer<CalendarController> localizer;
        private readonly IMapper mapper;

        public ViewModelHelper(IStringLocalizer<CalendarController> localizer, IMapper mapper)
        {
            this.localizer = localizer;
            this.mapper = mapper;
        }

        public AddEventViewModel CreateAddEventViewModel()
        {
            var viewModel = new AddEventViewModel();

            viewModel.IntendedFor = GetAttendeeType();
            viewModel.OccuringDays = GetOccuringDays();

            return viewModel;
        }
        public AddEventViewModel CreateAddEventViewModel(AddEventBindingModel addEventBindingModel)
        {
            var viewModel = this.mapper.Map<AddEventViewModel>(addEventBindingModel);

            viewModel.IntendedFor = new List<SelectListItem>();
            GetSelectedAttendeeType(viewModel, addEventBindingModel);

            viewModel.OccuringDays = new List<SelectListItem>();
            GetSelectedOccuringDays(viewModel, addEventBindingModel);

            return viewModel;
        }

        public FilterViewModel CreateFilterViewModel()
        {
            var viewModel = new FilterViewModel();

            viewModel.IntendedFor = GetAttendeeType();
            viewModel.OccuringDays = GetOccuringDays();

            return viewModel;
        }
        public FilterViewModel CreateFilterViewModel(FilterBindingModel filterBindingModel)
        {
            var viewModel = this.mapper.Map<FilterViewModel>(filterBindingModel);

            viewModel.IntendedFor = new List<SelectListItem>();
            GetSelectedAttendeeType(viewModel, filterBindingModel);

            viewModel.OccuringDays = new List<SelectListItem>();
            GetSelectedOccuringDays(viewModel, filterBindingModel);

            return viewModel;
        }

        public List<SelectListItem> GetOccuringDays()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (var dayName in Enum.GetValues(typeof(DaysOfWeek)))
            {
                var selectListItem = new SelectListItem();
                selectListItem.Value = ((int)dayName).ToString();
                selectListItem.Text = localizer[$"Day{dayName}"];
                result.Add(selectListItem);
            }
            return result;
        }

        public void GetSelectedOccuringDays(AddEventViewModel viewModel, AddEventBindingModel bindingModel)
        {
            viewModel.OccuringDays = GetOccuringDays();

            if (bindingModel.OccuringDays != null)
            {
                foreach (var dayName in viewModel.OccuringDays)
                {
                    dayName.Selected = false;

                    foreach (var selectedDay in bindingModel.OccuringDays)
                    {
                        if (dayName.Value.CompareTo(selectedDay.ToString()) == 0)
                        {
                            dayName.Selected = true;
                        }
                    }
                }
            }
        }
        public void GetSelectedOccuringDays(FilterViewModel viewModel, FilterBindingModel bindingModel)
        {
            viewModel.OccuringDays = GetOccuringDays();

            if (bindingModel.OccuringDays != null)
            {
                foreach (var dayName in viewModel.OccuringDays)
                {
                    dayName.Selected = false;

                    foreach (var selectedDay in bindingModel.OccuringDays)
                    {
                        if (dayName.Value.CompareTo(selectedDay.ToString()) == 0)
                        {
                            dayName.Selected = true;
                        }
                    }
                }
            }
        }

        public List<SelectListItem> GetAttendeeType()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (var type in Enum.GetValues(typeof(AttendeeType)))
            {
                var selectListItem = new SelectListItem();
                selectListItem.Value = ((int)type).ToString();
                selectListItem.Text = localizer[$"AttendeeType{type}"];
                result.Add(selectListItem);
            }
            return result;
        }

        public void GetSelectedAttendeeType(AddEventViewModel viewModel, AddEventBindingModel bindingModel)
        {
            viewModel.IntendedFor = GetAttendeeType();

            if (bindingModel.IntendedFor != null)
            {
                foreach (var attendee in viewModel.IntendedFor)
                {
                    attendee.Selected = false;

                    foreach (var selectedAttendee in bindingModel.IntendedFor)
                    {
                        if (attendee.Value.CompareTo(selectedAttendee.ToString()) == 0)
                        {
                            attendee.Selected = true;
                        }
                    }
                }
            }
        }
        public void GetSelectedAttendeeType(FilterViewModel viewModel, FilterBindingModel bindingModel)
        {
            viewModel.IntendedFor = GetAttendeeType();

            if (bindingModel.IntendedFor != null)
            {
                foreach (var attendee in viewModel.IntendedFor)
                {
                    attendee.Selected = false;

                    foreach (var selectedAttendee in bindingModel.IntendedFor)
                    {
                        if (attendee.Value.CompareTo(selectedAttendee.ToString()) == 0)
                        {
                            attendee.Selected = true;
                        }
                    }
                }
            }
        }

    }
}
