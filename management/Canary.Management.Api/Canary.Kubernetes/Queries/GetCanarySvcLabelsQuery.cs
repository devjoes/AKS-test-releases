using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.Kubernetes.Queries.Handlers;
using Rocket.Engine.Queries;

namespace Canary.Kubernetes.Queries
{
    public class GetCanarySvcLabelsQuery : IAsyncQuery<Dictionary<string,Dictionary<string,string>>>
    {
        private readonly IGetSvcLabelsQueryHandler  handler;
        
        public GetCanarySvcLabelsQuery(IGetSvcLabelsQueryHandler handler)
        {
            this.handler = handler;
        }

        public string Namespace { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Namespace))
            {
                throw new ArgumentException("missing namespace",nameof(this.Namespace));
            }
        }

        public async Task<Dictionary<string,Dictionary<string, string>>> Execute()
        {
            var labels = await this.handler.GetSvcLabels(this.Namespace);
            var canarySvcs = new Dictionary<string, Dictionary<string,string>>();
            foreach (var svc in labels.Keys)
            {
                var canaryLabels = labels[svc].Keys
                    .Where(l => l.ToLower().StartsWith("canary-"))
                    .ToDictionary(k => k, k => labels[svc][k]);
                if (canaryLabels.Any())
                {
                    canarySvcs.Add(svc, canaryLabels);
                }
            }

            return canarySvcs;
        }

    }
}
