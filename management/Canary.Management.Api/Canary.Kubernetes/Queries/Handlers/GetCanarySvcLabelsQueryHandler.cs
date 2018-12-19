using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using k8s;

namespace Canary.Kubernetes.Queries.Handlers
{
    public interface IGetSvcLabelsQueryHandler
    {
        Task<Dictionary<string, IDictionary<string, string>>> GetSvcLabels(string ns);
    }

    public class GetCanarySvcLabelsQueryHandler : IGetSvcLabelsQueryHandler
    {
        private readonly KubernetesClientConfiguration config;

        public GetCanarySvcLabelsQueryHandler(KubernetesClientConfiguration config)
        {
            this.config = config;
        }

        public async Task<Dictionary<string, IDictionary<string, string>>> GetSvcLabels(string ns)
        {
            using (var k8s = new k8s.Kubernetes(config))
            {
                var svcs = await k8s.ListNamespacedServiceAsync(ns);
                return svcs.Items.ToDictionary(p => p.Metadata.Name, p => p.Metadata.Labels);
            }
        }
    }
}
