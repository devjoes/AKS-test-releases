using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Commands;
using Canary.HaProxy.Commands.Handlers;
using Canary.HaProxy.Models;
using Canary.HaProxy.Queries;
using Canary.HaProxy.Queries.Handlers;
using Rocket.Engine.Contracts;

namespace Canary.HaProxy
{
    public interface IHaProxyActions
    {
        Task<IEnumerable<ConditionalRoute>> GetConditionalRoutes();
        Task<string[][]> RunCommand(string command);

        Task SetRouteCookieStrings(string hostname, string route, string[] cookieSubStrings);
        Task<bool> ReplacePlaceholderCookies(IEnumerable<ConditionalRoute> routes);

        Task<IEnumerable<Dictionary<string,string>>> GetStats();
    }

    public class HaProxyActions:IHaProxyActions
    {
        private readonly IQueryExecutor queryExecutor;
        private readonly ICommandExecutor commandExecutor;
        private readonly string haProxyHost;
        private readonly int haProxyPort;

        public HaProxyActions(string haProxyHostPort,IQueryExecutor queryExecutor, ICommandExecutor commandExecutor)
        {
            this.queryExecutor = queryExecutor;
            this.commandExecutor = commandExecutor;
            this.haProxyHost = haProxyHostPort.Split(":").First();
            this.haProxyPort = int.Parse(haProxyHostPort.Split(":").Last());
        }

        public async Task<IEnumerable<ConditionalRoute>> GetConditionalRoutes()
        {
            var query = new GetConditionalRoutesQuery(
                new GetConditionalRoutesQueryHandler(this.haProxyHost, this.haProxyPort));
            return await this.queryExecutor.ExecuteAsync(query);
        }

        public async Task<string[][]> RunCommand(string command)
        {
            var cmd = new ExecHaProxyCommand(
                new ExecHaProxyCommandHandler(this.haProxyHost, this.haProxyPort))
            {
                Command = command
            };
            await this.commandExecutor.ExecuteAsync(cmd);
            return cmd.Result;
        }

        public async Task SetRouteCookieStrings(string hostname, string route, string[] cookieSubStrings)
        {
            var cmd = new SetRouteCookiesCommand(
                new SetRouteCookiesCommandHandler(this, this.haProxyHost, this.haProxyPort))
            {
                Cookies = cookieSubStrings,
                HostName = hostname,
                RouteName = route
            };

            await this.commandExecutor.ExecuteAsync(cmd);
        }

        public async Task<bool> ReplacePlaceholderCookies(IEnumerable<ConditionalRoute> routes)
        {
            var cmd = new ReplacePlaceholderCookiesCommand(new ReplacePlaceholderCookiesCommandHandler(this))
            {
                Routes = routes
            };
            await this.commandExecutor.ExecuteAsync(cmd);
            return cmd.Result;
        }

        public async Task<IEnumerable<Dictionary<string, string>>> GetStats()
        {
            var query = new GetStatsQuery(new GetStatsQueryHandler(this.haProxyHost, this.haProxyPort));
            return await this.queryExecutor.ExecuteAsync(query);
        }
    }
}
