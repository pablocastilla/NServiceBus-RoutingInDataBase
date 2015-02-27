using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.DataAccess
{
    public class RoutingInfo
    {

        public string Assembly { get; set; }

        public string MessageType { get; set; }

        public string SourceMachine { get; set; }
        public string SourceComponent { get; set; }

        public string DestinationMachine { get; set; }

        public string DestinationEndpoint { get; set; }


    }
}
