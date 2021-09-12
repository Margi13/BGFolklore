using BGFolklore.Data.Models.Calendar;
using BGFolklore.Services.Public;
using BGFolklore.Services.Public.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGFolklore.Web.Common
{
    public static class Towns
    {

        public static ICollection<Town> AllTowns;
        public static ICollection<Town> AllAreas;

        public static void GetTowns(ITownsService townsService)
        {
            AllTowns = townsService.GetAllTowns();
            AllAreas = townsService.GetAllAreas();
        }
        public static ICollection<Town> GetTownsByAreaId(int areaId)
        {
            ICollection<Town> towns = new List<Town>();
            foreach (var town in AllTowns)
            {
                if(town.AreaId == areaId)
                {
                    towns.Add(town);
                }
            }
            return towns;
        }

    }
}
