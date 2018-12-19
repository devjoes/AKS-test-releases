using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using k8s;

namespace Canary.Kubernetes.Queries.Handlers
{
    public interface IGetNamespacesQueryHandler
    {
        Task<IEnumerable<string>> GetNamespaces();
    }
    public class GetNamespacesQueryHandler: IGetNamespacesQueryHandler
    {
        private readonly KubernetesClientConfiguration config;
        public GetNamespacesQueryHandler(KubernetesClientConfiguration config)
        {
            this.config = config;
        }

        public async Task<IEnumerable<string>> GetNamespaces()
        {
            using (var k8s = new k8s.Kubernetes(this.config))
            {
                var ns = await k8s.ListNamespaceAsync();
                return ns.Items.Select(n => n.Metadata.Name);
            }
        }
    }
}
