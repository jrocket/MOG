using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace MoG
{
    public class MogAuthAttribut: ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                                    typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {


                var user = filterContext.HttpContext.User;
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }
        
    }
}