using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Repository
{
    public class dbContextProvider : IdbContextProvider
    {
        public MogDbContext GetCurrent()
            {

                return (HttpContext.Current.Items["MogDbContext"] ??
                        (HttpContext.Current.Items["MogDbContext"] =
                        new MogDbContext())) as MogDbContext;

                
            }
      
    }


    public class dbContextProviderWithoutSingletonPerRequest : IdbContextProvider
    {
        private static MogDbContext _instance = null;


        public MogDbContext GetCurrent()
        {
            if (_instance == null)
            {
                _instance = new MogDbContext();
            }
            return _instance;

        }

      
    }
    public interface IdbContextProvider
    {
        MogDbContext GetCurrent();
    }
}