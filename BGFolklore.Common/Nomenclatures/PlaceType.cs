using System;
using System.ComponentModel.DataAnnotations;

namespace BGFolklore.Common.Nomenclatures
{
    public enum PlaceType
    {
        [Display(Name ="Отворено събитие")]
        Open = 1,

        [Display(Name = "Танцов състав")]
        Ensemble = 2,

        [Display(Name = "Заведение")]
        Restaurant = 3
    }
}
