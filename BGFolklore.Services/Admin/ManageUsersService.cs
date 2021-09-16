using AutoMapper;
using BGFolklore.Data;
using BGFolklore.Data.Models;
using BGFolklore.Models.Admin.ViewModels;
using BGFolklore.Services.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Admin
{
    public class ManageUsersService : BaseService, IManageUsersService
    {
        public ManageUsersService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IList<UserViewModel> GetAllUsers()
        {
            IList<UserViewModel> userViewModels = new List<UserViewModel>();

            var users = this.Context.Users;
            var allUsers = this.Mapper.Map<IEnumerable<User>>(users);

            foreach (var user in allUsers)
            {
                //user.AddEvents();
                //user.AddReports();
            }

            var rolesOfUser = this.Context.UserRoles.Where(ur => ur.UserId == "u");
            var usersWithRole = this.Context.UserRoles.Where(ur => ur.RoleId == "r");


            return userViewModels;
        }
    }
}
