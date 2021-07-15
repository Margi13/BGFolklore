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
            IList<Town> townsList = this.Mapper.Map<IList<Town>>(towns);
            return townsList;
        }

        public IList<Town> GetAllTownsFromGivenAreaId(int areaId)
        {
            throw new NotImplementedException();
        }

        public IList<Town> GetAllEventsFromGivenTownId(int townId)
        {
            throw new NotImplementedException();
        }

        public IList<Town> GetAllEventsFromGivenAreaId(int areaId)
        {
            throw new NotImplementedException();
        }

    }
}
