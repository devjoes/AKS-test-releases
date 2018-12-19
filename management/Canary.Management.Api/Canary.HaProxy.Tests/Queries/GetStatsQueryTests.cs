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
    public class GetStatsQueryIntegrationTests
    {
        [Fact]
        public async Task Execute_GetsStats_WhenExecuted()
        {
            const int port = 2468;
            using (var server = new TestServer(port))
            {
                server.Response = new Dictionary<string, string>()
                {
                    ["show stat"] = Constants.Stats
                };

                var query = new GetStatsQuery(new GetStatsQueryHandler(IPAddress.Loopback.ToString(), port));
                query.Validate();
                var stats = (await query.Execute()).ToArray();
                Assert.Equal("upstream-default-backend", stats.First()["pxname"]);
                Assert.Equal("FRONTEND", stats.Last()["svname"]);
            }
        }
    }
}
