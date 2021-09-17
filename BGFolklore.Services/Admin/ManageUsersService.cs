using AutoMapper;
using BGFolklore.Common;
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
        private readonly ICalendarService calendarService;
        private readonly IFeedbackService feedbackService;

        public ManageUsersService(ApplicationDbContext context, IMapper mapper, ICalendarService calendarService, IFeedbackService feedbackService) : base(context, mapper)
        {
            this.calendarService = calendarService;
            this.feedbackService = feedbackService;
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
                AddEventsToUser(user);
                AddReportsToUser(user);
                userViewModel = this.Mapper.Map<ManageUserViewModel>(user);

                var events = user.PublicEvents.Where(pe => pe.StatusId != (int)StatusName.Deleted).ToList();
                var reports = user.Reports.Where(e => e.StatusId != (int)StatusName.Deleted).ToList();

                if (events.Count != 0)
                {
                    userViewModel.ActivePublicEvents = this.Mapper.Map<IList<ManageEventViewModel>>(events);
                    AddOwnerName(userViewModel.ActivePublicEvents);
                    foreach (var eventViewModel in userViewModel.ActivePublicEvents)
                    {
                        AddEventFeedbacks(eventViewModel);
                    }
                }
                if (reports.Count != 0)
                {
                    userViewModel.ActiveReports = this.Mapper.Map<IList<ManageFeedbackViewModel>>(reports);
                    AddOwnerName(userViewModel.ActiveReports);
                    AddEventName(userViewModel.ActiveReports);
                }

                AddRolesToUserViewModel(userViewModel);
            }
            catch (Exception)
            {
                return new ManageUserViewModel();
            }

            return userViewModel;
        }
        private void AddEventFeedbacks(ManageEventViewModel eventViewModel)
        {
            var allEventFeedbacks = this.Context.Feedback.Where(f => f.EventId.Equals(eventViewModel.Id)).ToList();
            if (allEventFeedbacks != null)
            {
                eventViewModel.Feedbacks = this.Mapper.Map<IList<ManageFeedbackViewModel>>(allEventFeedbacks);
            }
            else
            {
                eventViewModel.Feedbacks = new List<ManageFeedbackViewModel>();
            }
        }
        private void AddEventName(IList<ManageFeedbackViewModel> reportViewModels)
        {
            foreach (var viewModel in reportViewModels)
            {
                var eventName = this.Context.PublicEvents.Where(e => e.Id.Equals(viewModel.EventId)).Select(e => e.Name).FirstOrDefault();
                if (eventName != null)
                {
                    viewModel.EventName = eventName.ToString();
                }
                else
                {
                    viewModel.EventName = "Не е намеренo!";
                }
            }
        }

        private void AddOwnerName(IList<ManageFeedbackViewModel> reportViewModels)
        {
            foreach (var viewModel in reportViewModels)
            {
                var user = GetUserById(viewModel.OwnerId);
                if (user != null)
                {
                    viewModel.OwnerUserName = user.UserName;
                }
                else
                {
                    viewModel.OwnerUserName = "Не е намерен!";
                }
            }
        }
        private void AddOwnerName(IList<ManageEventViewModel> eventViewModels)
        {
            foreach (var viewModel in eventViewModels)
            {
                var user = GetUserById(viewModel.OwnerId);
                if (user != null)
                {
                    viewModel.OwnerUserName = user.UserName;
                }
                else
                {
                    viewModel.OwnerUserName = "Не е намерен!";
                }
            }
        }
        private string GetRoleId(string roleName)
        {
            var roleId = this.Context.Roles.Where(r => r.Name.Equals(roleName)).Select(r => r.Id).FirstOrDefault();
            return roleId;
        }
        private void AddEventsToUser(User user)
        {
            try
            {
                var allUserEventsFromData = this.Context.PublicEvents.Where(pe => pe.OwnerId == user.Id).ToList();
                if (allUserEventsFromData != null)
                {
                    user.PublicEvents = this.Mapper.Map<IList<PublicEvent>>(allUserEventsFromData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void AddReportsToUser(User user)
        {
            try
            {
                var allUserReportsFromData = this.Context.Feedback.Where(f => f.OwnerId == user.Id).ToList();
                if (allUserReportsFromData != null)
                {
                    user.Reports = this.Mapper.Map<IList<Feedback>>(allUserReportsFromData);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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
        private void AddRolesToUserViewModel(ManageUserViewModel userViewModel)
        {
            userViewModel.Roles = new List<string>();
            var roleNames = GetUserRoles(userViewModel.Id);
            foreach (var roleName in roleNames)
            {
                userViewModel.Roles.Add(roleName);
            }
        }
        private User GetUserById(string userId)
        {
            User user = this.Context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();
            return user;
        }
    }
}
