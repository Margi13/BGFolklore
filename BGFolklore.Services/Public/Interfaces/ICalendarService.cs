﻿using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Models.Calendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public.Interfaces
{
    public interface ICalendarService
    {
        IList<UpcomingEventViewModel> GetUpcomingEvents();
        IList<RecurringEventViewModel> GetRecurringEvents();

        void SaveAddEvent(AddEventBindingModel newEvent);
    }
}