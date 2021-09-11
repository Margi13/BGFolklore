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

        public Town GetTownByGivenId(int id)
        {
            var towns = this.Context.Towns.Where(t => t.Id.Equals(id));
            if (towns != null)
            {
                Town townInfo = this.Mapper.Map<Town>(towns.First());
                return townInfo;
            }
            else
            {
                throw new Exception();
            }
        }
        public IList<Town> GetAllTownsByGivenAreaId(int areaId)
        {
            throw new NotImplementedException();
        }

        public IList<Town> GetAllEventsByGivenTownId(int townId)
        {
            throw new NotImplementedException();
        }

        public IList<Town> GetAllEventsByGivenAreaId(int areaId)
        {
            throw new NotImplementedException();
        }

    }
}
