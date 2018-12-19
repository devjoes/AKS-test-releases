using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.HaProxy.Models
{
    public class InvalidCookieSubstringException:Exception
    {
        public InvalidCookieSubstringException(string[] invalid): base(string.Join(",", invalid))
        {
            
        }
    }
}
