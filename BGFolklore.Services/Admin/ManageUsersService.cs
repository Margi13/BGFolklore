using AutoMapper;
using BGFolklore.Common;
using BGFolklore.Common.Common;
using BGFolklore.Common.Nomenclatures;
using BGFolklore.Data;
using BGFolklore.Data.Models;
using BGFolklore.Data.Models.Calendar;
using BGFolklore.Models.Admin.ViewModels;
using BGFolklore.Services.Admin.Interfaces;
using BGFolklore.Services.Public.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFolklore.Services.Admin
{
    public class ManageUsersService : BaseService, IManageUsersService
    {
        private readonly IManageEventsService manageEventsService;
        private readonly IManageFeedbacksService manageFeedsService;

        public ManageUsersService(ApplicationDbContext context, IMapper mapper, 
            IManageEventsService manageEventsService, 
            IManageFeedbacksService manageFeedsService) : base(context, mapper)
        {
            this.manageEventsService = manageEventsService;
            this.manageFeedsService = manageFeedsService;
        }

        public IList<ManageUserViewModel> GetAllUsers()
        {
            IList<ManageUserViewModel> resultViewModels = new List<ManageUserViewModel>();

            var users = this.Context.Users;
            var allUsers = this.Mapper.Map<IEnumerable<User>>(users);
            try
            {
                foreach (var user in allUsers)
                {
                    ManageUserViewModel userViewModel = GetUserViewModel(user);
                    resultViewModels.Add(userViewModel);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return resultViewModels;
        }
        public ManageUserViewModel GetUser(string userId)
        {
            ManageUserViewModel userViewModel = new ManageUserViewModel();
            var user = GetUserById(userId);
            if (user != null)
            {
                userViewModel = GetUserViewModel(user);
            }
            return userViewModel;
        }
        public void AddUserRole(string userId, string roleName)
        {

            var roleId = GetRoleId(roleName);

            if (roleId != null)
            {
                var hasUserRoleData = this.Context.UserRoles.Where(ur => ur.UserId.Equals(userId) && ur.RoleId.Equals(roleId)).Count();
                if (hasUserRoleData == 0)
                {
                    IdentityUserRole<string> userRole = new IdentityUserRole<string>();
                    userRole.UserId = userId;
                    userRole.RoleId = roleId;
                    this.Context.UserRoles.Add(userRole);
                    this.Context.SaveChanges();
                }
            }
        }
        public void RemoveUserRole(string userId, string roleName)
        {
            var roleId = GetRoleId(roleName);

            if (roleId != null)
            {
                var hasUserRoleData = this.Context.UserRoles.Where(ur => ur.UserId.Equals(userId)).Count();

                if (hasUserRoleData > 1)
                {
                    IdentityUserRole<string> userRole = new IdentityUserRole<string>();
                    userRole.UserId = userId;
                    userRole.RoleId = roleId;
                    this.Context.UserRoles.Remove(userRole);
                    this.Context.SaveChanges();
                }
            }
        }
        
        private ManageUserViewModel GetUserViewModel(User user)
        {
            ManageUserViewModel userViewModel = new ManageUserViewModel();
            userViewModel.ActivePublicEvents = new List<ManageEventViewModel>();
            userViewModel.ActiveReports = new List<ManageFeedbackViewModel>();
            try
            {
                manageEventsService.AddEventsToUser(user);
                manageFeedsService.AddReportsToUser(user);
                userViewModel = this.Mapper.Map<ManageUserViewModel>(user);

                var events = user.PublicEvents.Where(pe => pe.StatusId != (int)StatusName.Deleted).ToList();
                var reports = user.Reports.Where(e => e.StatusId != (int)StatusName.Deleted).ToList();

                if (events.Count != 0)
                {
                    userViewModel.ActivePublicEvents = this.Mapper.Map<IList<ManageEventViewModel>>(events);
                    manageEventsService.AddDataToEvent(userViewModel.ActivePublicEvents);
                    foreach (var eventViewModel in userViewModel.ActivePublicEvents)
                    {
                        manageFeedsService.AddEventFeedbacks(eventViewModel);
                    }
                }
                if (reports.Count != 0)
                {
                    userViewModel.ActiveReports = this.Mapper.Map<IList<ManageFeedbackViewModel>>(reports);
                    manageFeedsService.AddDataToFeedbacks(userViewModel.ActiveReports);
                }

                AddRolesToUserViewModel(userViewModel);
            }
            catch (Exception)
            {
                return new ManageUserViewModel();
            }

            return userViewModel;
        }

        private string GetRoleId(string roleName)
        {
            var roleId = this.Context.Roles.Where(r => r.Name.Equals(roleName)).Select(r => r.Id).FirstOrDefault();
            return roleId;
        }
        private void AddRolesToUserViewModel(ManageUserViewModel userViewModel)
        {
            userViewModel.Roles = new List<string>();
            var roleNames = GetUserRoles(userViewModel.Id);
            foreach (var roleName in roleNames)
            {
                userViewModel.Roles.Add(roleName);
            }
        }
        private IList<string> GetUserRoles(string userId)
        {
            var userRolesFromData = this.Context.UserRoles.Where(ur => ur.UserId.Equals(userId)).ToList();
            IList<string> roles = new List<string>();
            foreach (var userRole in userRolesFromData)
            {
                var role = this.Context.Roles.Where(r => r.Id.Equals(userRole.RoleId)).FirstOrDefault();
                if (role != null)
                {
                    roles.Add(role.Name);
                }
            }
            roles = roles.OrderBy(r => r).ToList();
            return roles;
        }
        private User GetUserById(string userId)
        {
            User user = this.Context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();
            user.Email = EncryptDecrypt.Decryption(user.Email);
            return user;
        }
    }
}
