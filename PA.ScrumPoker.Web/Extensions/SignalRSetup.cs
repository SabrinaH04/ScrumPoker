using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PA.ScrumPoker.Web.SignalRHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PA.ScrumPoker.Web.Extensions
{
    internal static class SignalRSetup
    {
        public static bool IsAzureSetup { get; set; } = false;

        public static IApplicationBuilder SetupAppForSignalR(this IApplicationBuilder app)
        {
            return IsAzureSetup ?
                   app.UseAzureSignalR(route => { route.MapHub<ProjectHub>("/scrumpoker/notify"); }) :
                   app.UseSignalR(route => { route.MapHub<ProjectHub>(string.Concat("/scrumpoker/notify")); });
        }

        public static IServiceCollection SetupServicesForSignalR(this IServiceCollection services)
        {
            //var signalRConnectionString = EnvironmentVariables.GetSecretOrEnvVar("CommonEntityWeb_SignalRConnectionString");
            var signalRConnectionString = "";

            if (!string.IsNullOrWhiteSpace(signalRConnectionString))
            {
                IsAzureSetup = true;
                services.AddSignalR().AddAzureSignalR(signalRConnectionString);
            }
            else
            {
                services.AddSignalR();
            }
            return services;
        }
    }
}
