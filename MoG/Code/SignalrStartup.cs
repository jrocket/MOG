
using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace MoG.Code
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
} 

  

    public class MyConnection : PersistentConnection 
    {
        protected override Task OnReceived(IRequest request, string connectionId, string data) 
        {
            // Broadcast data to all clients
            return Connection.Broadcast(data);
        }
    }