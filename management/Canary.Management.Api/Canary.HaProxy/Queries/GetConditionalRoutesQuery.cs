using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Canary.HaProxy.Models;
using Canary.HaProxy.Queries.Handlers;
using Rocket.Engine.Queries;

namespace Canary.HaProxy.Queries
{
    public class GetConditionalRoutesQuery : IAsyncQuery<IEnumerable<ConditionalRoute>>
    {
        private readonly IGetConditionalRoutesQueryHandler handler;

        public GetConditionalRoutesQuery(IGetConditionalRoutesQueryHandler handler)
        {
            this.handler = handler;
        }

        public void Validate()
        {
        }

        public async Task<IEnumerable<ConditionalRoute>> Execute()
        {
            var allAcls = await this.handler.GetAllAcls();
            var aclIds = GetRelevantAclIds(allAcls).ToArray();
            //var tmp = allAcls.Keys.Select(a => this.handler.GetAclValues(a).Result).ToArray(); //TODO: Remove
            string[] hosts = null;
            List<ConditionalRoute> routes = new List<ConditionalRoute>();
            for (int i = 0; i < aclIds.Length; i += 3)
            {
                var route = new ConditionalRoute();
                var urls = (await this.handler.GetAclValues(aclIds[i + 1])).Where(u => u.StartsWith("/")).ToArray();
                route.RouteAlias = urls.Length == 1
                    ? urls.Single()
                    : urls.Single(u => Regex.IsMatch(u, Constants.ConditionalRouteRegex));
                route.Url = urls.Length == 1
                    ? urls.Single()
                    : urls.Single(u => !Regex.IsMatch(u, Constants.ConditionalRouteRegex));
                route.Cookies = (await this.handler.GetAclValues(aclIds[i + 2])).ToArray();
                route.CookieAclIndex = aclIds[i + 2];
                var possHosts = (await this.handler.GetAclValues(aclIds[i])).ToArray();
                if (isHosts(possHosts))
                {
                    hosts = possHosts;
                }
                route.Hosts = hosts;
                routes.Add(route);
            }

            return routes;
        }

        private static bool isHosts(string[] possHosts)
        {
            return
                possHosts.Length > 1 &&
                possHosts.Count(h =>
                           h.Contains(":") &&
                           h.Substring(h.IndexOf(":") + 1).Trim().All(char.IsDigit)
                       ) == possHosts.Length - 1;
        }

        private static IEnumerable<int> GetRelevantAclIds(Dictionary<int, string> allAcls)
        {
            var nonSslAclIds = allAcls
                .Where(kvp => !kvp.Value.Contains("ssl"))
                .Select(kvp => kvp.Key)
                .OrderByDescending(i => i)
                .ToArray();
            foreach (var aclIndex in allAcls.Keys)
            {
                if (allAcls[aclIndex].Contains("'req.cook'"))
                {
                    //TODO: This logic only works if conditional routes are alphabetically first
                    int aclBeforeIndex = nonSslAclIds.First(i => i < aclIndex);
                    yield return nonSslAclIds.First(i => i < aclBeforeIndex);
                    yield return aclBeforeIndex;
                    yield return aclIndex;
                }
            }
        }
    }
}
