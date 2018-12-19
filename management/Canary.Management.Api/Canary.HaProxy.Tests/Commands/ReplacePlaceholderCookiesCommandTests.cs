using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Commands;
using Canary.HaProxy.Commands.Handlers;
using Canary.HaProxy.Models;
using Moq;
using Xunit;

namespace Canary.HaProxy.Tests.Commands
{
    public class ReplacePlaceholderCookiesCommandTests
    {
        [Fact]
        public void Validate_Errors_WhenRoutesIsNull()
        {
            var cmd = new ReplacePlaceholderCookiesCommand(null);
            Assert.Throws<ArgumentException>(() => cmd.Validate());
        }

        [Fact]
        public async Task Execute_DoesNothing_WhenNoPlaceholderCookiesFound()
        {
            var actions = new Mock<IHaProxyActions>();
            var cmd = BuildReplacePlaceholderCookiesCommand(actions, "blah");
            await cmd.Execute();

            actions.Verify(a => a.SetRouteCookieStrings(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>()),
                Times.Never);
            Assert.False(cmd.Result);
        }

        [Fact]
        public async Task Execute_ReplacesCookie_WhenPlaceholderCookiesFound()
        {
            var actions = new Mock<IHaProxyActions>();
            var cmd = BuildReplacePlaceholderCookiesCommand(actions, ReplacePlaceholderCookiesCommandHandler.PlaceHolderCookie);
            await cmd.Execute();

            actions.Verify(a => a.SetRouteCookieStrings("foo.bar", "/"+HaProxy.Constants.ConditionalRouteSuffix+"", It.IsAny<string[]>()), Times.Once);
            Assert.True(cmd.Result);
        }

        private static ReplacePlaceholderCookiesCommand BuildReplacePlaceholderCookiesCommand(Mock<IHaProxyActions> actions, string cookie)
        {
            var routes =
                new[]
                {
                    new ConditionalRoute
                    {
                        Hosts = new[] {"foo.bar"}, RouteAlias = "/"+HaProxy.Constants.ConditionalRouteSuffix+"", Url = "/", CookieAclIndex = 1,
                        Cookies = new[] {cookie}
                    }
                };
            var cmd = new ReplacePlaceholderCookiesCommand(new ReplacePlaceholderCookiesCommandHandler(actions.Object))
            {
                Routes = routes
            };
            cmd.Validate();
            return cmd;
        }
    }
}
