using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Code
{
    public class SecurityService
    {
        private delegate bool IsPermissionBaseGranted(Object context, UserProfileInfo user);

        private Dictionary<MoGPermission, IsPermissionBaseGranted> permissionEvaluator = null;
        public SecurityService()
        {
            permissionEvaluator = new Dictionary<MoGPermission, IsPermissionBaseGranted>();
            permissionEvaluator.Add(MoGPermission.EditFile, isEditFileGranted);
        }

        private bool isEditFileGranted(object context, UserProfileInfo user)
        {
            ProjectFile f = context as ProjectFile;
            if (f==null || f.Creator == null || user == null)
            {
                return false;
            }
            return f.Creator.Id == user.Id;

        }

        public bool IsPermissionGranted(MoGPermission perm, Object context,UserProfileInfo user)
        {
            return permissionEvaluator[perm](context, user);
        }

    }

    public enum MoGPermission
    {
        EditFile = 0,
        DeleteFile = 1

    }
}