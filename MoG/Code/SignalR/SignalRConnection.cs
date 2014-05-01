using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Code
{
    public class signalRConnections
    {
        private static ConnectionMapping<string> _instance = null;


        public static ConnectionMapping<string> Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConnectionMapping<string>();
                return _instance;
            }

        }


    }

}