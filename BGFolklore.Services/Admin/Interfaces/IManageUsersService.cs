﻿using BGFolklore.Models.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Admin.Interfaces
{
    public interface IManageUsersService
    {
        IList<UserViewModel> GetAllUsers();
    }
}