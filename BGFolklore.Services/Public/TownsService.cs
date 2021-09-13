using AutoMapper;
using BGFolklore.Data;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Services.Public.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public
{
    public class TownsService : BaseService, ITownsService
    {
        public TownsService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IList<Town> GetAllTowns()
        {
            var towns = this.Context.Towns.OrderBy(t => t.Name);
            if (towns != null)
            {
                IList<Town> townsList = this.Mapper.Map<IList<Town>>(towns);
                return townsList;
            }
            else
            {
                throw new Exception();
            }
        }
        public IList<Town> GetAllAreas()
        {
            var towns = this.Context.Towns.Where(t => t.Id == t.AreaId).OrderBy(t => t.Name);
            if (towns != null)
            {
                IList<Town> townsList = this.Mapper.Map<IList<Town>>(towns);
                return townsList;
            }
            else
            {
                throw new Exception();
            }
        }
        public Town GetTownByGivenId(int id)
        {
            Town town = new Town();
            var towns = this.Context.Towns.Where(t => t.Id == id).FirstOrDefault();
            if (towns != null)
            {
                town = this.Mapper.Map<Town>(towns);
            }
            else
            {
                throw new Exception();
            }

            return town;
        }
        public IList<Town> GetAllTownsByGivenAreaId(int areaId)
        {
            IList<Town> townsList = new List<Town>();
            var towns = this.Context.Towns.Where(t => t.AreaId == areaId);
            if (towns != null)
            {
                townsList = this.Mapper.Map<IList<Town>>(towns);
            }
            else
            {
                throw new Exception();
            }

            return townsList;
        }

        public IList<PublicEvent> GetAllEventsByGivenAreaId(int areaId, IEnumerable<PublicEvent> listToFilter)
        {
            IList<PublicEvent> publicEvents = new List<PublicEvent>();
            var townsInArea = GetAllTownsByGivenAreaId(areaId);
            foreach (var town in townsInArea)
            {
                IList<PublicEvent> eventsByTown = GetAllEventsByGivenTownId(town.Id, listToFilter);

                if (eventsByTown != null)
                {
                    foreach (var ev in eventsByTown)
                    {
                        publicEvents.Add(ev);
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            return publicEvents;
        }

        public IList<PublicEvent> GetAllEventsByGivenTownId(int townId, IEnumerable<PublicEvent> listToFilter)
        {
            IList<PublicEvent> publicEvents = new List<PublicEvent>();
            var eventsByTown = listToFilter.Where(t => t.TownId == townId);
            if (eventsByTown != null)
            {
                publicEvents = this.Mapper.Map<IList<PublicEvent>>(eventsByTown);
            }
            else
            {
                throw new Exception();
            }

            return publicEvents;
        }
    }
}
