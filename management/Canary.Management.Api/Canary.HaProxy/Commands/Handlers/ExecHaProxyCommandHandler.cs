using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Canary.HaProxy.Commands.Handlers
{
    public interface IExecHaProxyCommandHandler
    {
        Task<byte[]> SendCommandToServer(string command);
    }
    public class ExecHaProxyCommandHandler : BaseHandler, IExecHaProxyCommandHandler
    {
        public ExecHaProxyCommandHandler(string host, int port) : base(host, port)
        {
        }

        public async Task<byte[]> SendCommandToServer(string command)
        {
            return await this.SendTextAndReadResponse(command);
        }
    }
}
