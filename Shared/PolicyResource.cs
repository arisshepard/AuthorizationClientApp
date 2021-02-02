using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorizationClientApp.Shared
{
    public class PolicyResource
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Id { get; set; }
    }
}
