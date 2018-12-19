using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Management.Models
{
    public class Site
    {
        public string Host { get; set; }
        public IEnumerable<Route> Routes { get; set; }
    }
}
