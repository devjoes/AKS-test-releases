using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Models;

namespace Canary.HaProxy.Commands.Handlers
{
    public interface IReplacePlaceholderCookiesCommandHandler
    {
        Task<bool> ReplacePlaceholderCookies(IEnumerable<ConditionalRoute> routes);
    }

    public class ReplacePlaceholderCookiesCommandHandler : IReplacePlaceholderCookiesCommandHandler
    {
        public const string PlaceHolderCookie = "change_me";
        private readonly IHaProxyActions actions;

        public ReplacePlaceholderCookiesCommandHandler(IHaProxyActions actions)
        {
            this.actions = actions;
        }


        public async Task<bool> ReplacePlaceholderCookies(IEnumerable<ConditionalRoute> routes)
        {
            var routesWithPlaceholderCookies =
                routes.Where(r => r.Cookies.SingleOrDefault(c => c == PlaceHolderCookie) != null).ToArray();
            foreach (var route in routesWithPlaceholderCookies)
            {
                await this.actions.SetRouteCookieStrings(route.Hosts.First(), route.RouteAlias,
                    new[] { "p" + Guid.NewGuid() });
            }

            return routesWithPlaceholderCookies.Any();
        }
    }
}
