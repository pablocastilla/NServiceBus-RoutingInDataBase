using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.DataAccess
{
    public class EventSubscription
    {
        public string EventType { get; set; }

        public string SubscriberEndpoint { get; set; }
        public string SubscriberMachine { get; set; }
    }
}
