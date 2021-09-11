using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public.Interfaces
{
    public interface IRatingService
    {
        void SaveRating(RatingBindingModel ratingBindingModel);

        float GetEventRatingsAverage(Guid eventId);
    }
}
