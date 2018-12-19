using System.Collections.Generic;
using Canary.HaProxy.Models;
using Newtonsoft.Json;

namespace Canary.Management.Models
{
    public class Route
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string[] CookieMustContain { get; set; }
        [JsonIgnore]
        public ConditionalRoute ConditionalRoute { get; set; }
    }
}