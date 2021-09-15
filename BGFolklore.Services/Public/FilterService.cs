using AutoMapper;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public
{
    public class FilterService : BaseService, IFilterService
    {
        private readonly ITownsService townsService;

        public FilterService(ApplicationDbContext context, IMapper mapper,
            ITownsService townsService) : base(context, mapper)
        {
            this.townsService = townsService;
        }

        public IList<UpcomingEventViewModel> GetFilteredUpcomingEvents(FilterBindingModel filterBindingModel, IEnumerable<PublicEvent> listToFilter)
        {
            IList<UpcomingEventViewModel> resultUpcomingEvents;
            try
            {
                IEnumerable<PublicEvent> events = GetFilteredData(filterBindingModel, listToFilter);

                events = OrderFilteredData(filterBindingModel.OwnerId, filterBindingModel.IsRecurring, events);
                if (events == null)
                {
                    return new List<UpcomingEventViewModel>();
                }
                var resultEvents = this.Mapper.Map<IEnumerable<UpcomingEventViewModel>>(events);
                resultUpcomingEvents = this.Mapper.Map<IList<UpcomingEventViewModel>>(resultEvents);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return resultUpcomingEvents;
        }
        public IList<RecurringEventViewModel> GetFilteredRecurringEvents(FilterBindingModel filterBindingModel, IEnumerable<PublicEvent> listToFilter)
        {
            IList<RecurringEventViewModel> resultRecurringEvents;
            try
            {
                IEnumerable<PublicEvent> events = GetFilteredData(filterBindingModel, listToFilter);
                if (events == null)
                {
                    return new List<RecurringEventViewModel>();
                }
                resultRecurringEvents = this.Mapper.Map<IList<RecurringEventViewModel>>(events);
            }
            catch (Exception)
            {
                throw;
            }

            //Order events
            return resultRecurringEvents;
        }

        public IEnumerable<PublicEvent> OrderFilteredData(Guid? ownerId, bool isRecurring, IEnumerable<PublicEvent> listToOrder)
        {
            if (listToOrder == null)
            {
                return null;
            }

            IEnumerable<PublicEvent> ordered = new List<PublicEvent>();

            if (ownerId == null)
            {
                if (isRecurring)
                {
                    ordered = listToOrder.OrderByDescending(e => e.Rating);
                }
                else
                {
                    ordered = listToOrder.OrderByDescending(e => e.EventDateTime);
                }
            }
            else
            {
                if (isRecurring)
                {
                    ordered = listToOrder.OrderByDescending(e =>
                    {
                        if (e.Feedbacks != null)
                        {
                            foreach (var feed in e.Feedbacks)
                            {
                                if (feed.StatusId == (int)StatusName.New)
                                {
                                    return true;
                                }
                            }
                        }
                        return false;
                    }).ThenByDescending(e => e.Rating);
                }
                else
                {
                    ordered = listToOrder.OrderByDescending(e =>
                    {
                        if (e.Feedbacks != null)
                        {
                            foreach (var feed in e.Feedbacks)
                            {
                                if (feed.StatusId == (int)StatusName.New)
                                {
                                    return true;
                                }
                            }
                        }
                        return false;
                    })
                    .ThenBy(e => e.EventDateTime);
                }

            }
            return ordered;
        }

        private IEnumerable<PublicEvent> GetFilteredData(FilterBindingModel filterBindingModel, IEnumerable<PublicEvent> listToFilter)
        {
            IEnumerable<PublicEvent> events = listToFilter;
            try
            {
                if (filterBindingModel.OwnerId != null)
                {
                    events = GetOwnersEvents(filterBindingModel.OwnerId, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }

                if (filterBindingModel.TownId != 0)
                {
                    events = townsService.GetAllEventsByGivenTownId(filterBindingModel.TownId, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }
                else if (filterBindingModel.AreaId != 0)
                {
                    events = townsService.GetAllEventsByGivenAreaId(filterBindingModel.AreaId, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }

                if (filterBindingModel.PlaceType != null)
                {
                    events = GetEventsByPlaceType(filterBindingModel.PlaceType, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }

                if (filterBindingModel.AfterDate != null)
                {
                    events = GetEventsAfterGivenDate(filterBindingModel.AfterDate, filterBindingModel.IsRecurring, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }
                else if (filterBindingModel.BeforeDate != null)
                {
                    events = GetEventsBeforeGivenDate(filterBindingModel.BeforeDate, filterBindingModel.IsRecurring, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }

                if (filterBindingModel.IntendedFor != null)
                {
                    events = GetEventsByIntendedFor(filterBindingModel.IntendedFor, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }
                if (filterBindingModel.OccuringDays != null)
                {
                    events = GetEventsByOccurringDays(filterBindingModel.OccuringDays, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }
                if (filterBindingModel.DurationInDays != 0)
                {
                    events = GetEventsByDuration(filterBindingModel.DurationInDays, events);
                    if (events == null)
                    {
                        return new List<PublicEvent>();
                    }
                }
                events = this.Mapper.Map<IList<PublicEvent>>(events);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return events;
        }

        private IEnumerable<PublicEvent> GetOwnersEvents(Guid? ownerId, IEnumerable<PublicEvent> listToFilter)
        {
            IEnumerable<PublicEvent> filtered = new List<PublicEvent>();
            if (ownerId != null)
            {
                try
                {
                    filtered = listToFilter.Where(e => e.OwnerId.Equals(ownerId.ToString()));
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            return filtered;
        }

        private IEnumerable<PublicEvent> GetEventsByPlaceType(PlaceType? placeType, IEnumerable<PublicEvent> listToFilter)
        {
            IEnumerable<PublicEvent> filtered = new List<PublicEvent>();
            if (placeType != null)
            {
                try
                {
                    filtered = listToFilter.Where(e => e.PlaceType.Equals(placeType));
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            return filtered;
        }

        private IEnumerable<PublicEvent> GetEventsAfterGivenDate(DateTime? after, bool isRecurring, IEnumerable<PublicEvent> listToFilter)
        {
            IEnumerable<PublicEvent> filtered = new List<PublicEvent>();
            if (after != null)
            {
                try
                {
                    if (isRecurring)
                    {
                        filtered = listToFilter.Where(e => e.EventDateTime.TimeOfDay.CompareTo(after.Value.TimeOfDay) >= 0);
                    }
                    else
                    {
                        filtered = listToFilter.Where(e => e.EventDateTime.CompareTo(after) >= 0);
                    }
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            return filtered;
        }

        private IEnumerable<PublicEvent> GetEventsBeforeGivenDate(DateTime? before, bool isRecurring, IEnumerable<PublicEvent> listToFilter)
        {
            IEnumerable<PublicEvent> filtered = new List<PublicEvent>();
            if (before != null)
            {
                try
                {
                    if (isRecurring)
                    {
                        filtered = listToFilter.Where(e => e.EventDateTime.TimeOfDay.CompareTo(before.Value.TimeOfDay) <= 0);
                    }
                    else
                    {
                        filtered = listToFilter.Where(e => e.EventDateTime.CompareTo(before) <= 0);
                    }
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            return filtered;
        }

        private IEnumerable<PublicEvent> GetEventsByIntendedFor(int[] intendedFor, IEnumerable<PublicEvent> listToFilter)
        {
            IEnumerable<PublicEvent> filtered = new List<PublicEvent>();
            if (intendedFor != null && intendedFor.Length != 0)
            {
                try
                {
                    filtered = listToFilter.Where(e =>
                    {
                        foreach (var attendeeType in intendedFor)
                        {
                            if ((e.IntendedFor & attendeeType) != 0)
                            {
                                return true;
                            }
                        }
                        return false;
                    });
                }
                catch (Exception)
                {
                    throw new Exception();
                }

            }
            return filtered;

        }

        private IEnumerable<PublicEvent> GetEventsByOccurringDays(int[] occurringDays, IEnumerable<PublicEvent> listToFilter)
        {
            IEnumerable<PublicEvent> filtered = new List<PublicEvent>();
            if (occurringDays != null && occurringDays.Length != 0)
            {
                try
                {
                    filtered = listToFilter.Where(e =>
                    {
                        foreach (var day in occurringDays)
                        {
                            if ((e.OccuringDays & day) != 0)
                            {
                                return true;
                            }
                        }
                        return false;
                    });
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            return filtered;
        }

        private IEnumerable<PublicEvent> GetEventsByDuration(int durationInDays, IEnumerable<PublicEvent> listToFilter)
        {
            IEnumerable<PublicEvent> filtered = new List<PublicEvent>();
            if (durationInDays != 0)
            {
                try
                {
                    filtered = listToFilter.Where(e => e.DurationInDays == durationInDays);
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            return filtered;
        }
    }
}
