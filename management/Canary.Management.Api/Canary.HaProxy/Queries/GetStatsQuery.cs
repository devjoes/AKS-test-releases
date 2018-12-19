using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy.Queries.Handlers;
using Rocket.Engine.Queries;

namespace Canary.HaProxy.Queries
{
    public class GetStatsQuery : IAsyncQuery<IEnumerable<Dictionary<string,string>>>
    {
        private readonly IGetStatsQueryHandler handler;

        public GetStatsQuery(IGetStatsQueryHandler handler)
        {
            this.handler = handler;
        }

        public void Validate()
        {
            
        }

        public async Task<IEnumerable<Dictionary<string, string>>> Execute()
        {
            return await this.handler.GetStats();
        }
    }
}
