using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace MoG.Code
{
    public class notificationHub : Hub
    {


        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            signalRConnections.Instance.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            string name = Context.User.Identity.Name;

            signalRConnections.Instance.Remove(name, Context.ConnectionId);

            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            if ( Context.User !=null &&  Context.User.Identity!=null)
            {
                string name = Context.User.Identity.Name;

                if (!signalRConnections.Instance.GetConnections(name).Contains(Context.ConnectionId))
                {
                    signalRConnections.Instance.Add(name, Context.ConnectionId);
                }

            }
           
            return base.OnReconnected();
        }
    }


    
}