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
        Town GetTownByGivenId(int id);
        IList<Town> GetAllTownsByGivenAreaId(int areaId);
        IList<Town> GetAllEventsByGivenTownId(int townId);
        IList<Town> GetAllEventsByGivenAreaId(int areaId);
    }
}
