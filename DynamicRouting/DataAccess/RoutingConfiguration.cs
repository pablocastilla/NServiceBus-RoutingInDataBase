using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.DataAccess
{
    public class RoutingConfiguration
    {
        /// <summary>
        /// Assembly name of the message
        /// </summary>
        public string MessageAssembly { get; set; }

        /// <summary>
        /// Message type
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Name of the machine that sends
        /// </summary>
        public string SourceMachine { get; set; }

        /// <summary>
        /// Name of the component that sends
        /// </summary>
        public string SourceComponent { get; set; }

        /// <summary>
        /// Destination machine
        /// </summary>
        public string DestinationMachine { get; set; }


        /// <summary>
        /// Destination endpoint (queue).
        /// </summary>
        public string DestinationEndpoint { get; set; }


    }
}
