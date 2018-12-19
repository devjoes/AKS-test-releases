using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.Kubernetes.Models;
using Canary.Kubernetes.Queries;
using Canary.Kubernetes.Queries.Handlers;
using k8s;
using Rocket.Engine.Contracts;

namespace Canary.Kubernetes
{
    public interface IKubernetesActions
    {
        Task<Dictionary<string, IEnumerable<Backend>>> GetIngressPathsInNamespace(string k8SNamespace);
        Task<Dictionary<string, Dictionary<string, string>>> GetCanarySvcLabels(string k8SNamespace);
        Task<IEnumerable<string>> GetNamespaces();
    }
    public class KubernetesActions : IKubernetesActions
    {
        public KubernetesActions(KubernetesClientConfiguration config, IQueryExecutor queryExecutor, ICommandExecutor commandExecutor)
        {
            this.config = config;
            this.queryExecutor = queryExecutor;
            this.commandExecutor = commandExecutor;
        }
        private readonly KubernetesClientConfiguration config;
        private readonly ICommandExecutor commandExecutor;
        private readonly IQueryExecutor queryExecutor;

        public async Task<Dictionary<string, IEnumerable<Backend>>> GetIngressPathsInNamespace(string k8SNamespace)
        {
            var query = new GetIngressPathsInNamespaceQuery(new GetIngressPathsInNamespaceQueryHandler(this.config))
            {
                Namespace = k8SNamespace
            };

            return await this.queryExecutor.ExecuteAsync(query);
        }

        public async Task<Dictionary<string, Dictionary<string, string>>> GetCanarySvcLabels(string k8SNamespace)
        {

            var query = new GetCanarySvcLabelsQuery(new GetCanarySvcLabelsQueryHandler(this.config))
            {
                Namespace = k8SNamespace
            };

            return await this.queryExecutor.ExecuteAsync(query);
        }

        public async Task<IEnumerable<string>> GetNamespaces()
        {
            var query = new GetNamespacesQuery(new GetNamespacesQueryHandler(this.config));

            return await this.queryExecutor.ExecuteAsync(query);
        }
    }
}
