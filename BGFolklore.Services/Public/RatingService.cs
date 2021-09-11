using AutoMapper;
using BGFolklore.Data;
using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Calendar.BindingModels;
using BGFolklore.Services.Public.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public
{
    public class RatingService : BaseService, IRatingService
    {
        public RatingService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }


        public float GetEventRatingsAverage(Guid eventId)
        {
            var ratingsFromData = this.Context.Rating.Where(r => r.EventId == eventId);
            if (ratingsFromData == null)
            {
                throw new Exception();
            }
            float averageRating = 0;
            try
            {
                IList<Rating> ratings = this.Mapper.Map<IList<Rating>>(ratingsFromData);
                int count = 0;
                foreach (var rating in ratings)
                {
                    averageRating += rating.Rate;
                    count++;
                }
                averageRating = averageRating / (float)count;
            }
            catch (Exception)
            {

                throw;
            }
            return averageRating;
        }

        public void SaveRating(RatingBindingModel ratingBindingModel)
        {
            try
            {
                var hasRatingFromUser = this.Context.Rating.Where(r => r.OwnerId == ratingBindingModel.OwnerId && r.EventId == ratingBindingModel.EventId).FirstOrDefault();
                if (hasRatingFromUser != null)
                {
                    Rating ratingToUpdate = this.Mapper.Map<Rating>(hasRatingFromUser);
                    UpdateOwnRateForEvent(ratingToUpdate, ratingBindingModel.Rate);
                }
                else
                {
                    Rating rating = this.Mapper.Map<Rating>(ratingBindingModel);
                    var pe = this.Context.PublicEvents.Where(pe => pe.Id == ratingBindingModel.EventId).FirstOrDefault();
                    var ow = this.Context.Users.Where(u => u.Id == ratingBindingModel.OwnerId).FirstOrDefault();

                    PublicEvent publicEvent = this.Mapper.Map<PublicEvent>(pe);
                    var owner = this.Mapper.Map<User>(ow);

                    if (publicEvent == null || owner == null)
                    {
                        throw new Exception();
                    }

                    rating.Event = publicEvent;
                    rating.Owner = owner;

                    this.Context.Rating.Add(rating);
                    this.Context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private Rating GetRatingById(Guid ratingId)
        {
            var ratingFromData = this.Context.Rating.Where(r => r.Id == ratingId).FirstOrDefault();
            if (ratingFromData == null)
            {
                throw new Exception();
            }
            Rating rating = new Rating();
            try
            {
                rating = this.Mapper.Map<Rating>(ratingFromData);
            }
            catch (Exception)
            {

                throw;
            }
            return rating;
        }
        private void UpdateOwnRateForEvent(Rating ratingToUpdate, int rate)
        {
            try
            {
                ratingToUpdate.Rate = rate;
                this.Context.Rating.Update(ratingToUpdate);
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
