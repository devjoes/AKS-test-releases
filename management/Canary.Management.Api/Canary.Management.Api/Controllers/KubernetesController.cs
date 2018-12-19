using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Canary.HaProxy;
using Canary.Kubernetes;
using Microsoft.AspNetCore.Mvc;

namespace Canary.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KubernetesController : ControllerBase
    {
        private readonly IKubernetesActions kubernetesActions;
        private IHaProxyActions haProxyActions;

        public KubernetesController(IKubernetesActions kubernetesActions, IHaProxyActions haProxyActions)
        {
            this.kubernetesActions = kubernetesActions;
            this.haProxyActions = haProxyActions;
        }
        [HttpGet()]
        public async Task<ActionResult> Get()
        {
            return new JsonResult( await this.kubernetesActions.GetNamespaces());

            //var config = await this.haProxyActions.GetConditionalRoutes();
            //var config = await this.kubernetesActions.GetIngressPathsInNamespace(kubernetesNamespace);
            //return new JsonResult(config);
            //return new JsonResult(await this.kubernetesActions.GetCanarySvcLabels(kubernetesNamespace));
        }
    }
}
