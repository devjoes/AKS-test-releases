using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.HaProxy.Models
{
    public class RouteNotFoundException:Exception
    {
        public RouteNotFoundException(string hostname, string route)
            :base($"Could not find route {hostname}/{route}")
        {
            
        }
    }
}
