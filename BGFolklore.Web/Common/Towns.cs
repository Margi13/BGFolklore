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

        public static void GetTowns(ITownsService townsService)
        {
            AllTowns = townsService.GetAllTowns();
        }

    }
}
