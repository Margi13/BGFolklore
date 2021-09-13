using BGFolklore.Data.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Public.Interfaces
{
    public interface ITownsService
    {
        IList<Town> GetAllTowns();
        IList<Town> GetAllAreas();
        Town GetTownByGivenId(int id);
        IList<Town> GetAllTownsByGivenAreaId(int areaId);
        IList<PublicEvent> GetAllEventsByGivenTownId(int townId, IEnumerable<PublicEvent> listToFilter);
        IList<PublicEvent> GetAllEventsByGivenAreaId(int areaId, IEnumerable<PublicEvent> listToFilter);
    }
}
