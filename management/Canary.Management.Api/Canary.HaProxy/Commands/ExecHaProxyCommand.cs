using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Commands.Handlers;
using Rocket.Engine.Commands;

namespace Canary.HaProxy.Commands
{
    public class ExecHaProxyCommand : IAsyncCommand<string[][]>
    {
        private readonly IExecHaProxyCommandHandler handler;

        public ExecHaProxyCommand(IExecHaProxyCommandHandler handler)
        {
            this.handler = handler;
        }

        public async Task Execute()
        {
            var data = await this.handler.SendCommandToServer(this.Command);
            var text = Encoding.ASCII.GetString(data);
            this.Result = text
                .Split("\n\n")
                .Select(c => c
                    .Split("\n")
                    .Select(l => l.Trim())
                    .ToArray()
                ).ToArray();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Command))
            {
                throw new ArgumentException("missing command", nameof(this.Command));
            }
        }

        public string[][] Result { get; private set; }
        public string Command { get; set; }
    }
}
