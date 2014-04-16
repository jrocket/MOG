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
            this.serviceProject = projectService;
            this.serviceInvit = invitService;
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
