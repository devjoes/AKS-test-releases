using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Canary.HaProxy;
using Canary.Management.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Canary.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HaProxyController: ControllerBase
    {
        private IHaProxyActions actions;
        private ICachingService cachingService;

        public HaProxyController(IHaProxyActions actions, ICachingService cachingService)
        {
            this.actions = actions;
            this.cachingService = cachingService;
        }

        [HttpPut("[action]")]
        public async Task Route([FromBody] SetRouteRequest routeRequest)
        {
            await this.actions.SetRouteCookieStrings(routeRequest.Hostname, routeRequest.RouteName,
                routeRequest.CookieSubStrings);
            await this.cachingService.Clear();
        }

    }
}
