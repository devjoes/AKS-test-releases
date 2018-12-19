using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Commands;
using Canary.HaProxy.Commands.Handlers;
using Canary.HaProxy.Models;
using Rocket.Engine.Services;
using Xunit;

namespace Canary.HaProxy.Tests.Commands
{
    public class SetRouteCookiesCommandTests
    {
        [InlineData(null, "/app", new[] { "foo" })]
        [InlineData("", "/app", new[] { "foo" })]
        [InlineData("foo.com", null, new[] { "foo" })]
        [InlineData("foo.com", "", new[] { "foo" })]
        [InlineData("foo.com", "/app", null)]
        [InlineData("foo.com", "/app", new string[0])]
        [Theory]
        public void Validate_Errors_WhenParamsMissing(string hostname, string routeName, string[] cookies)
        {
            var cmd = new SetRouteCookiesCommand(null)
            {
                HostName = hostname,
                RouteName = routeName,
                Cookies = cookies
            };

            Assert.Throws<ArgumentException>(() => cmd.Validate());
        }

        [Theory]
        [InlineData("blah blah")]
        [InlineData("blah;blah")]
        public void Validate_Errors_WhenCookieStringInvalid(string cookieString)
        {
            var cmd = new SetRouteCookiesCommand(null)
            {
                HostName = "foo.bar",
                RouteName = "/",
                Cookies = new []{cookieString}
            };

            Assert.Throws<InvalidCookieSubstringException>(() => cmd.Validate());
        }
    }

    [Collection("SequentialServerTests")]
    public class SetRouteCookiesCommandIntegrationTests
    {
        [Theory]
        [InlineData("not_found", "not_found")]
        [InlineData("foo.bar", "not_found")]
        [InlineData("not_found", "/app1"+HaProxy.Constants.ConditionalRouteSuffix+"")]
        public async Task Execute_Errors_IfRouteIsNotFound(string hostname, string routeName)
        {
            const int port = 2468;
            using (var server = new TestServer(port))
            {
                setResponses(server);
                var actions = new HaProxyActions(IPAddress.Loopback + ":" + port, new QueryExecutor(), new CommandExecutor());
                var cmd = new SetRouteCookiesCommand(new SetRouteCookiesCommandHandler(actions, IPAddress.Loopback.ToString(), port))
                {
                    HostName = hostname,
                    RouteName = routeName,
                    Cookies = new[] { "foo" }
                };

                cmd.Validate();
                await Assert.ThrowsAsync<RouteNotFoundException>(() => cmd.Execute());
            }
        }

        [Fact]
        public async Task Execute_SendsCookiesToServer_IfRouteExists()
        {
            const int port = 2468;
            using (var server = new TestServer(port))
            {
                setResponses(server);
                var actions = new HaProxyActions(IPAddress.Loopback + ":" + port, new QueryExecutor(), new CommandExecutor());
                var cmd = new SetRouteCookiesCommand(new SetRouteCookiesCommandHandler(actions, IPAddress.Loopback.ToString(), port))
                {
                    HostName = "foo.bar",
                    RouteName = "/"+HaProxy.Constants.ConditionalRouteSuffix+"",
                    Cookies = new[] { "foo" }
                };

                cmd.Validate();
                await cmd.Execute();

                Assert.Contains("clear acl #8", server.RecievedText.Trim());
                Assert.All(cmd.Cookies, c => Assert.Contains("add acl #8 " + c, server.RecievedText));
            }
        }

        private static void setResponses(TestServer server)
        {
            server.Response = new Dictionary<string, string>()
            {
                {"show acl", Constants.TwoConditionalRoutesWithSSl.ShowAclResponse},
                {"show acl #3", Constants.TwoConditionalRoutesWithSSl.ShowAcl3Response},
                {"show acl #4", Constants.TwoConditionalRoutesWithSSl.ShowAcl4Response},
                {"show acl #5", Constants.TwoConditionalRoutesWithSSl.ShowAcl5Response},
                {"show acl #7", Constants.TwoConditionalRoutesWithSSl.ShowAcl7Response},
                {"show acl #8", Constants.TwoConditionalRoutesWithSSl.ShowAcl8Response}
            };
        }
    }
}
