using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Common.Nomenclatures
{
    public enum CustomRoleNames
    {
        [Display(Name = "Администратор")]
        Administrator = 1,

        [Display(Name = "Модератор")]
        Moderator = 2,

        [Display(Name = "Танцова формация")]
        Organization = 3,

        [Display(Name = "Обикновен потребител")]
        User = 4
    }
}
