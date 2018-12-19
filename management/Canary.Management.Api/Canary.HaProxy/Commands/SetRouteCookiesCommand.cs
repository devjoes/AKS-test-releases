using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Commands.Handlers;
using Canary.HaProxy.Models;
using Rocket.Engine.Commands;

namespace Canary.HaProxy.Commands
{
    public class SetRouteCookiesCommand : IAsyncCommand
    {
        private readonly ISetRouteCookiesCommandHandler handler;

        public SetRouteCookiesCommand(ISetRouteCookiesCommandHandler handler)
        {
            this.handler = handler;
        }
        public async Task Execute()
        {
            await this.handler.SetCookiesForRoute(this.HostName, this.RouteName, this.Cookies);
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.HostName))
            {
                throw new ArgumentException("hostname is required", nameof(this.HostName));
            }
            if (string.IsNullOrWhiteSpace(this.RouteName))
            {
                throw new ArgumentException("route name is required", nameof(this.RouteName));
            }
            if (this.Cookies == null || !this.Cookies.Any())
            {
                throw new ArgumentException("cookies is required", nameof(this.Cookies));
            }

            var invalidCookies = this.Cookies.Where(c => c.Contains(" ") || c.Contains(";")).ToArray();
            if (invalidCookies.Any())
            {
                throw new InvalidCookieSubstringException(invalidCookies);
            }
        }

        public string HostName { get; set; }
        public string RouteName { get; set; }
        public string[] Cookies { get; set; }
    }
}
