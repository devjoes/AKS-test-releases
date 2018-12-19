using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy;
using Canary.Kubernetes;
using Canary.Management.Models;
using Canary.Management.Queries;
using Canary.Management.Queries.Handlers;
using Rocket.Engine.Contracts;

namespace Canary.Management
{
    public interface IManagementActions
    {
        Task<IEnumerable<Site>> GetSites(string ns);
    }
    public class ManagementActions:IManagementActions
    {
        private readonly IHaProxyActions haProxyActions;
        private readonly IKubernetesActions kubernetesActions;
        private readonly ICommandExecutor commandExecutor;
        private readonly IQueryExecutor queryExecutor;

        public ManagementActions(IQueryExecutor queryExecutor, ICommandExecutor commandExecutor,IKubernetesActions kubernetesActions, IHaProxyActions haProxyActions)
        {
            this.kubernetesActions = kubernetesActions;
            this.haProxyActions = haProxyActions;
            this.queryExecutor = queryExecutor;
            this.commandExecutor = commandExecutor;
        }

        public async Task<IEnumerable<Site>> GetSites(string ns)
        {
            var query = new GetSitesQuery(new GetSitesQueryHandler(this.kubernetesActions, this.haProxyActions))
            {
                Namespace = ns
            };

            return await this.queryExecutor.ExecuteAsync(query);
        }
    }
}
