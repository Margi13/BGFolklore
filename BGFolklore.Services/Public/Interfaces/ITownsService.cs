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
        IList<Town> GetAllTownsFromGivenAreaId(int areaId);
        IList<Town> GetAllEventsFromGivenTownId(int townId);
        IList<Town> GetAllEventsFromGivenAreaId(int areaId);
    }
}
