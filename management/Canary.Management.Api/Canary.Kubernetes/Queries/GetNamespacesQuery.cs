using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.Kubernetes.Queries.Handlers;
using Rocket.Engine.Queries;

namespace Canary.Kubernetes.Queries
{
    public class GetNamespacesQuery : IAsyncQuery<IEnumerable<string>>
    {
        private readonly IGetNamespacesQueryHandler handler;

        public GetNamespacesQuery(IGetNamespacesQueryHandler handler)
        {
            this.handler = handler;
        }

        public void Validate()
        {
        }

        public async Task<IEnumerable<string>> Execute()
        {
            return await this.handler.GetNamespaces();
        }
    }
}
