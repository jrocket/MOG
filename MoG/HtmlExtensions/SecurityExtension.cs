using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MoG
{
    public static class SecurityExtension
    {
        public static bool HasRight(
            this HtmlHelper htmlHelper,
            object context,
            SecureActivity activity)
        {
            return true;
            //SecurityService service = new SecurityService();
        }
    }
}