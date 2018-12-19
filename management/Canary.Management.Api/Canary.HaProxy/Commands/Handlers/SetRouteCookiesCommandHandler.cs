using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Models;
using Rocket.Engine.Contracts;

namespace Canary.HaProxy.Commands.Handlers
{
    public interface ISetRouteCookiesCommandHandler
    {
        Task SetCookiesForRoute(string hostname, string routeName, string[] cookies);
    }

    public class SetRouteCookiesCommandHandler : BaseHandler, ISetRouteCookiesCommandHandler
    {
        private readonly IHaProxyActions actions;

        public SetRouteCookiesCommandHandler(IHaProxyActions actions,string host, int port) : base(host, port)
        {
            this.actions = actions;
        }

        public async Task SetCookiesForRoute(string hostname, string routeName, string[] cookies)
        {
            int index = await this.GetRouteIndex(hostname, routeName);
            string cmd = $"clear acl #{index};" + 
                         string.Join(";", cookies.Select(c => $"add acl #{index} {c}"));
            string response = Encoding.ASCII.GetString(await this.SendTextAndReadResponse(cmd));
            
        }

        private async Task<int> GetRouteIndex(string hostname, string routeName)
        {
            var routes = await this.actions.GetConditionalRoutes();
            var route = routes.SingleOrDefault(r =>
                r.Hosts.Any(h => h.Equals(hostname, StringComparison.InvariantCultureIgnoreCase))
                && r.RouteAlias.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));
            if (route == null)
            {
                throw new RouteNotFoundException(hostname, routeName);
            }

            return route.CookieAclIndex;
        }
    }
}
