using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.HaProxy.Models
{
    public class ConditionalRoute
    {
        public string RouteAlias { get; set; }
        public string Url { get; set; }
        public string[] Cookies { get; set; }
        public string[] Hosts { get; set; }
        public int CookieAclIndex { get; set; }
    }
}
