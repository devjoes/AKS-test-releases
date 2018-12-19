using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.Management.Models;
using Canary.Management.Queries.Handlers;
using Rocket.Engine.Queries;

namespace Canary.Management.Queries
{
    public class GetSitesQuery : IAsyncQuery<IEnumerable<Site>>
    {
        public GetSitesQuery(IGetHostsQueryHandler handler)
        {
            this.handler = handler;
        }

        private readonly IGetHostsQueryHandler handler;
        public string Namespace { get; set; }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Namespace))
            {
                throw new ArgumentException("missing namespace", nameof(this.Namespace));
            }
        }

        public async Task<IEnumerable<Site>> Execute()
        {
            return await this.handler.GetSites(this.Namespace);
        }
    }
}
