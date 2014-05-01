using Microsoft.AspNet.SignalR;
using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Code
{
    public class NotificationSignal
    {
        // Singleton instance
        public readonly static Lazy<NotificationSignal> _instance = new Lazy<NotificationSignal>(
            () => new NotificationSignal(GlobalHost.ConnectionManager.GetHubContext<notificationHub>()));

        private IHubContext _context;

        private NotificationSignal(IHubContext context)
        {
            _context = context;
        }

        public void NotifySomeone(string login,string message)
        {

            foreach (var connectionId in signalRConnections.Instance.GetConnections(login))
            {
                _context.Clients.Client(connectionId).addNotification(message);
            }

        }
    }

}