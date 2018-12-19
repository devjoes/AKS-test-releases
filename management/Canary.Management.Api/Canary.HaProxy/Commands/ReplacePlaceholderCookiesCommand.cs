using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Commands.Handlers;
using Canary.HaProxy.Models;
using Rocket.Engine.Commands;

namespace Canary.HaProxy.Commands
{
    public class ReplacePlaceholderCookiesCommand : IAsyncCommand<bool>
    {
        private IReplacePlaceholderCookiesCommandHandler handler;

        public ReplacePlaceholderCookiesCommand(IReplacePlaceholderCookiesCommandHandler handler)
        {
            this.handler = handler;
        }

        public IEnumerable<ConditionalRoute> Routes { get; set; }

        public async Task Execute()
        {
            this.Result = await this.handler.ReplacePlaceholderCookies(this.Routes);
        }

        public void Validate()
        {
            if (this.Routes == null)
            {
                throw new ArgumentException("routes is required", nameof(this.Routes));
            }
        }

        public bool Result { get; private set; }
    }
}
