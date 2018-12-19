using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Canary.Management.Api.Models
{
    public class SetRouteRequest
    {
        public string Hostname { get; set; }
        public string RouteName { get; set; }
        public string[] CookieSubStrings { get; set; }
    }
}
