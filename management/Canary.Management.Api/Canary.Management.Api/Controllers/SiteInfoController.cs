using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Canary.HaProxy;
using Canary.Kubernetes;
using Canary.Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Canary.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteInfoController : ControllerBase
    {
        private readonly IManagementActions managementActions;
        private readonly IHaProxyActions haProxyActions;
        private readonly IKubernetesActions k8sActions;
        private readonly ICachingService cachingService;

        public SiteInfoController(IManagementActions managementActions, IHaProxyActions haProxyActions, IKubernetesActions k8sActions, ICachingService cachingService)
        {
            this.managementActions = managementActions;
            this.haProxyActions = haProxyActions;
            this.k8sActions = k8sActions;
            this.cachingService = cachingService;
        }

        [HttpGet("{ns}")]
        public async Task<IEnumerable<Site>> Get(string ns)
        {
            return await this.cachingService.TryGetFromCache(async () =>
                {
                    var siteInfo = (await this.managementActions.GetSites(ns)).ToArray();
                    var conditionalRoutes = siteInfo
                        .SelectMany(s => s.Routes)
                        .Select(r => r.ConditionalRoute)
                        .Where(r => r != null);
                    if (await this.haProxyActions.ReplacePlaceholderCookies(conditionalRoutes))
                    {
                        siteInfo = (await this.managementActions.GetSites(ns)).ToArray();
                    }

                    return siteInfo;
                }, "siteinfo_" + ns);
        }


        [HttpGet("[action]/{ns}")]
        public async Task<Dictionary<string, int>> SessionsPerRoute(string ns)
        {
            string removePort(string str)
            {
                return Regex.Replace(str, "[\\:\\-]\\d+$", String.Empty);
            }

            var stats = await this.haProxyActions.GetStats();
            var ingresses = await this.k8sActions.GetIngressPathsInNamespace(ns);
            return ingresses.SelectMany(i =>
            {
                return i.Value.Select(v =>
                (
                    Host: i.Key,
                    Path: v.Path,
                    Service: v.Service
                ));
            }).ToDictionary(
                k => k.Host + k.Path,
                v => int.Parse(stats.SingleOrDefault(
                                   s => s["svname"] != null &&
                                        s["svname"].Equals("backend", StringComparison.InvariantCultureIgnoreCase) &&
                                        removePort(s["pxname"]).Equals(ns + "-" + removePort(v.Service), StringComparison.InvariantCultureIgnoreCase))
                                   ?["stot"] ?? "0"));
        }
    }
}
