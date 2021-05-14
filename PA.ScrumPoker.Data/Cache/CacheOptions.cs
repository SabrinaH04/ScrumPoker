using System;
using System.Collections.Generic;
using System.Text;

namespace PA.ScrumPoker.Data.Cache
{
    public class CacheOptions
    {
        public bool Enabled { get; set; }
        public TimeSpan Expiration { get; set; }
    }
}
