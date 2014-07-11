using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public enum SecureActivity
    {
        ProjectCreate,
        ProjectDelete,
        ProjectEdit,
        ProjectView,
        TrackCreate,
        TrackEdit,
        TrackDelete,
        UserProfileCreate,
        UserProfileDelete,
        UserProfileEdit,
        MessageCreate,
        MessageDelete,
        MessageEdit,
        CommentEdit,
        ViewUserDashboard,
    }

    public class SecurityService : ISecurityService
    {
        private Dictionary<SecureActivity, HasRight_Delegate> hasRightMethods = null;
        delegate bool HasRight_Delegate(UserProfileInfo user,object context);
        private IProjectService serviceProject = null;
        private IInvitService serviceInvit = null; 

        public SecurityService( IProjectService projectService,
            IInvitService invitService)
        {
            hasRightMethods = new Dictionary<SecureActivity, HasRight_Delegate>();
            hasRightMethods.Add(SecureActivity.ProjectEdit, CanEditProject);
            hasRightMethods.Add(SecureActivity.ProjectView, CanViewProject);
            hasRightMethods.Add(SecureActivity.ProjectDelete, CanDeleteProject);
            hasRightMethods.Add(SecureActivity.TrackEdit, CanEditTrack);
            hasRightMethods.Add(SecureActivity.TrackDelete, CanDeleteTrack);
            hasRightMethods.Add(SecureActivity.CommentEdit, CanEditComment);
            hasRightMethods.Add(SecureActivity.ViewUserDashboard, CanViewUserDashboard);

            this.serviceProject = projectService;
            this.serviceInvit = invitService;
        }

    


        private bool CanEditComment(UserProfileInfo user, object context)
        {
            bool result = false;
            Comment comment = context as Comment;
            if (comment != null)
            {
                result = comment.Creator.Id == user.Id;
            }
            return result;
        }

        private bool CanViewUserDashboard(UserProfileInfo user, object context)
        {
            int userId = (int)context;
            return user.Id == userId;
        }

        private bool CanDeleteTrack(UserProfileInfo user, object context)
        {
            bool result = false;
            ProjectFile file = context as ProjectFile;
            if (file != null)
            {
                result = file.Creator.Id == user.Id;
                result |= serviceProject.IsOwner(file.Project, user);
            }
            return result;
        }

        private bool CanEditTrack(UserProfileInfo user, object context)
        {
            bool result = false;
            ProjectFile file = context as ProjectFile;
            if (file != null)
            {
                result = file.Creator.Id == user.Id;
            }
            return result;
        }

        private bool CanDeleteProject(UserProfileInfo user, object context)
        {
            return CanEditProject(user, context);
        }

        private bool CanViewProject(UserProfileInfo user, object context)
        {
            bool result = false;
            Project project = context as Project;
            if (project != null)
            {
                result = project.VisibilityType == Visibility.Public;
                result |= serviceProject.IsOwner(project, user);
                result |= serviceInvit.IsInvited(project, user);
            }
            return result;
        }

        private bool CanEditProject(UserProfileInfo user, object context)
        {
            bool result = false;
            Project project = context as Project;
            if (project!=null)
            {
                result = serviceProject.IsOwner(project,user);
            }
            return result;
        }

        public bool HasRight(SecureActivity activity, UserProfileInfo user, object context)
        {
            if (user == null)
            {
                return false;
            }
            if (this.hasRightMethods.ContainsKey(activity))
            {
                return this.hasRightMethods[activity](user, context);
            }
            else
                return false;
        }
    }

    public interface ISecurityService
    {
        bool HasRight(SecureActivity activity, UserProfileInfo user, object context);

    }
}
