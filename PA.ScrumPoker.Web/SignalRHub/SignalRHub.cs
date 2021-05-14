using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PA.ScrumPoker.Web.SignalRHub
{
    public class ProjectHub : Hub<IHubClient>
    {
        protected static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ProjectHub() { }

        public override Task OnConnectedAsync()
        {
            try
            {
                Clients.Caller.SendID(Context.ConnectionId);
                logger.Info($"HubClient send ConnectionId: {Context.ConnectionId}");
            }
            catch (Exception ex)
            {
                logger.Error($"HubClient send connectionId: {Context.ConnectionId} failed. Error: {ex}");
            }

            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            logger.Info($"HubClient send ConnectionId : {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task RefreshRoom(string message)
        {
            await Clients.All.RefreshRoom(message);
        }
    }
}
