using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace HoiTroWebsite.Hubs
{
    [HubName("notificationHub")]
    public class NotificationHub:Hub
    {
        public void NotifyAdmins(string message)
        {
            Clients.All.receiveNotification(message);
        }
    }
}