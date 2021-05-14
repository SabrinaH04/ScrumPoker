using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PA.ScrumPoker.Web.SignalRHub
{
    public interface IHubClient
    {
        Task SendID(string id);
        Task SendAlert(string message);

        Task RefreshRoom(string message);
    }
}
