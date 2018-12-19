using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Commands;
using Canary.HaProxy.Commands.Handlers;
using Xunit;

namespace Canary.HaProxy.Tests.Commands
{
    [Collection("SequentialServerTests")]
    public class ExecHaProxyCommandTests
    {

        [Fact]
        public async Task Execute_ValidCommand_SendsDataToServer()
        {
            const int port = 2468;
            using (var server = new TestServer(port))
            {
                var cmd = new ExecHaProxyCommand(new ExecHaProxyCommandHandler(IPAddress.Loopback.ToString(), port))
                {
                    Command = "test command"
                };

                cmd.Validate();
                await cmd.Execute();
                Assert.Equal(cmd.Command + "\n", server.RecievedText);
            }
        }

        [Theory]
        [InlineData("test command", "foo\nbar\nfoo", "foo\nbar\nfoo")]
        [InlineData("test command;test command", "foo\nbar\nfoo", "foo\nbar\nfoo|foo\nbar\nfoo")]
        public async Task Execute_ValidCommand_RecievesDataFromServer(string cmdText, string cmdResponse, string cmdExpected)
        {
            const int port = 2468;
            using (var server = new TestServer(port))
            {
                server.Response = new Dictionary<string, string>()
                {
                    {cmdText.Split(";").First(), cmdResponse.Split("|").First()}
                };
                var cmd = new ExecHaProxyCommand(new ExecHaProxyCommandHandler(IPAddress.Loopback.ToString(), port))
                {
                    Command = cmdText
                };

                cmd.Validate();
                await cmd.Execute();
                Assert.Equal(cmdExpected, string.Join("|",cmd.Result.Select(i => string.Join("\n", i))));
            }
        }
    }
}
