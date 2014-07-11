using MoG.Code;
using System.Web;
using System.Web.Mvc;

namespace MoG
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

            //filters.Add(new HandleErrorAttribute());
            filters.Add(
                new MogErrorHandler(
                    new MoG.Domain.Service.LogService(
                        new MoG.Domain.Repository.LogRepository(new MoG.Domain.Repository.dbContextProvider())
                        )
                        )
                        );
        }
    }
}
