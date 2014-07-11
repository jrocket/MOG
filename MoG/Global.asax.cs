
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using MoG.Domain.Service;
using MoG.Domain.Models;

namespace MoG
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Data.Entity.Database.SetInitializer(new MyDataContextDbInitializer());
            MogAutomapper.RegisterAutomapper();
            IScheduledTaskService scheduledtasks = DependencyResolver.Current.GetService<IScheduledTaskService>();
            //var scheduledtasks = new Service.ScheduledTaskService(HttpRuntime.Cache);
            scheduledtasks.StartService();
        }
    }
}
