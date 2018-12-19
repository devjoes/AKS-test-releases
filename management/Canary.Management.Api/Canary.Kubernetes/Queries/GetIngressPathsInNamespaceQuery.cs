using Canary.Kubernetes.Queries.Handlers;
using Rocket.Engine.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.Kubernetes.Models;

namespace Canary.Kubernetes.Queries
{
    public class GetIngressPathsInNamespaceQuery : IAsyncQuery<Dictionary<string, IEnumerable<Backend>>>
    {
        private readonly IGetIngressPathsInNamespaceQueryHandler handler;

        public GetIngressPathsInNamespaceQuery(IGetIngressPathsInNamespaceQueryHandler handler)
        {
            this.handler = handler;
        }

        public string Namespace { get; set; }

        public async Task<Dictionary<string, IEnumerable<Backend>>> Execute()
        {
            return await this.handler.GetIngressPathsInNamespace(this.Namespace);
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Namespace))
            {
                throw new ArgumentException("missing namespace", nameof(this.Namespace));
            }
        }
    }
}
