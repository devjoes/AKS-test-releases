using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Queries;
using Canary.HaProxy.Queries.Handlers;
using Xunit;

namespace Canary.HaProxy.Tests.Queries
{
    [Collection("SequentialServerTests")]
    public class GetConditionalRoutesQueryIntegrationTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("blah")]
        public async Task Execute_ReadsRoutesFromServer_WhenSuppliedWithRouteNames(string routeSubstring)
        {
            const int port = 2468;
            using (var server = new TestServer(port))
            {
                server.Response = new Dictionary<string, string>()
                {
                    { "show acl", Constants.TwoConditionalRoutesWithSSl.ShowAclResponse},
                    { "show acl #3", Constants.TwoConditionalRoutesWithSSl.ShowAcl3Response},
                    { "show acl #4", Constants.TwoConditionalRoutesWithSSl.ShowAcl4Response.Replace(HaProxy.Constants.ConditionalRouteSuffix, "_" + routeSubstring + "conditional")},
                    { "show acl #5", Constants.TwoConditionalRoutesWithSSl.ShowAcl5Response},
                    { "show acl #7", Constants.TwoConditionalRoutesWithSSl.ShowAcl7Response.Replace(HaProxy.Constants.ConditionalRouteSuffix, "_" + routeSubstring + "conditional")},
                    { "show acl #8", Constants.TwoConditionalRoutesWithSSl.ShowAcl8Response}
                };
                var query = new GetConditionalRoutesQuery(
                    new GetConditionalRoutesQueryHandler(IPAddress.Loopback.ToString(), port));
                query.Validate();
                var routes = (await query.Execute()).ToList();

                Assert.Collection(routes, r =>
                {
                    Assert.Equal("/app1_" + routeSubstring + "conditional", r.RouteAlias);
                    Assert.Equal("/app1", r.Url);
                    Assert.Contains("foo", r.Cookies);
                    Assert.Contains("bar", r.Cookies);
                    Assert.Equal(2, r.Cookies.Length);
                    Assert.Equal(5, r.CookieAclIndex);
                }, r =>
                {
                    Assert.Equal("/_" + routeSubstring + "conditional", r.RouteAlias);
                    Assert.Equal("/", r.Url);
                    Assert.Equal("foo", r.Cookies.Single());
                    Assert.Equal(8, r.CookieAclIndex);
                });
                Assert.All(routes, r =>
                {
                    Assert.Equal(3, r.Hosts.Length);
                    Assert.Equal("foo.bar", r.Hosts[0]);
                    Assert.Equal("foo.bar:80", r.Hosts[1]);
                    Assert.Equal("foo.bar:443", r.Hosts[2]);
                });
            }
        }

        [Fact]
        public async Task GetAllAcls_ReadsAclIdsAndText_WhenSuppliedWithAcls()
        {
            const int port = 2468;
            using (var server = new TestServer(port))
            {
                server.Response = new Dictionary<string, string>()
                {
                    {"show acl", Constants.TwoConditionalRoutesWithSSl.ShowAclResponse}
                };
                var handler = new GetConditionalRoutesQueryHandler(IPAddress.Loopback.ToString(), port);
                var acls = await handler.GetAllAcls();
                for (int i = 0; i <= 10; i++)
                {
                    Assert.Equal(i, acls.Keys.ToArray()[i]);
                }
                Assert.Contains("'req.cook'", acls[5]);
            }
        }


        [Fact]
        public async Task GetAclValues_ReadsValues_WhenSuppliedWithAclIndexes()
        {
            const int port = 2468;
            using (var server = new TestServer(port))
            {
                server.Response = new Dictionary<string, string>()
                {
                    {"show acl #5", Constants.TwoConditionalRoutesWithSSl.ShowAcl5Response}
                };
                var handler = new GetConditionalRoutesQueryHandler(IPAddress.Loopback.ToString(), port);
                var values = (await handler.GetAclValues(5)).ToArray();
                Assert.Equal(2, values.Length);
                Assert.Equal("foo", values[0]);
                Assert.Equal("bar", values[1]);
            }
        }
    }
}
