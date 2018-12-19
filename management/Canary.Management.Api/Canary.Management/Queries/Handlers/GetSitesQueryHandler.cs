using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy;
using Canary.HaProxy.Models;
using Canary.Kubernetes;
using Canary.Management.Models;

namespace Canary.Management.Queries.Handlers
{
    public interface IGetHostsQueryHandler
    {
        Task<IEnumerable<Site>> GetSites(string ns);
    }
    public class GetSitesQueryHandler : IGetHostsQueryHandler
    {
        private readonly IHaProxyActions haProxyActions;
        private readonly IKubernetesActions kubernetesActions;

        public GetSitesQueryHandler(IKubernetesActions kubernetesActions, IHaProxyActions haProxyActions)
        {
            this.kubernetesActions = kubernetesActions;
            this.haProxyActions = haProxyActions;
        }
        public async Task<IEnumerable<Site>> GetSites(string ns)
        {
            var ingresses = await this.kubernetesActions.GetIngressPathsInNamespace(ns);
            var conditionalRoutes = (await this.haProxyActions.GetConditionalRoutes()).ToArray();
            var sites = ingresses.Keys.Select(k => 
                new Site
                {
                    Host = k,
                    Routes = this.GetRoutes(k, ingresses[k].Select(i => i.Path), conditionalRoutes).ToArray()
                }).ToArray();
            
            return sites;
        }

        private IEnumerable<Route> GetRoutes(string host, IEnumerable<string> routes, ConditionalRoute[] conditionalRoutes)
        {
            return routes.Select(r =>
            {
                var condRoute = conditionalRoutes.SingleOrDefault(c =>
                    c.Hosts.Any(h => h.Equals(host, StringComparison.InvariantCultureIgnoreCase))
                    && c.RouteAlias.Equals(r, StringComparison.InvariantCultureIgnoreCase));
                if (condRoute == null)
                {
                    return new Route
                    {
                        Name = r,
                        Url = r,
                        CookieMustContain = new string[0]
                    };
                }

                return new Route
                {
                    Url = condRoute.Url,
                    Name = condRoute.RouteAlias,
                    CookieMustContain = condRoute.Cookies,
                    ConditionalRoute = condRoute
                };
            });
        }
    }
}
