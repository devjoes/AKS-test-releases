using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Kubernetes.Models
{
    public class Backend
    {
        public string Path { get; set; }
        public string Service { get; set; }
    }
}
