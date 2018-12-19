using k8s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.Kubernetes.Models;

namespace Canary.Kubernetes.Queries.Handlers
{
    public interface IGetIngressPathsInNamespaceQueryHandler
    {
        Task<Dictionary<string, IEnumerable<Backend>>> GetIngressPathsInNamespace(string ns);
    }

    public class GetIngressPathsInNamespaceQueryHandler : IGetIngressPathsInNamespaceQueryHandler
    {
        private readonly KubernetesClientConfiguration config;

        public GetIngressPathsInNamespaceQueryHandler(KubernetesClientConfiguration config)
        {
            this.config = config;
        }
        public async Task<Dictionary<string, IEnumerable<Backend>>> GetIngressPathsInNamespace(string ns)
        {
            using (var k8s = new k8s.Kubernetes(this.config))
            {
                var ings = await k8s.ListNamespacedIngressAsync(ns);
                return ings.Items
                    .Where(i => i.Metadata.NamespaceProperty == ns)
                    .SelectMany(i => i.Spec.Rules)
                    .ToDictionary(k => k.Host, r => r.Http.Paths
                        .Select(p => new Backend{Path = p.Path, Service = $"{p.Backend.ServiceName}:{p.Backend.ServicePort.Value}" })
                        .Distinct());
            }
        }
    }
}
