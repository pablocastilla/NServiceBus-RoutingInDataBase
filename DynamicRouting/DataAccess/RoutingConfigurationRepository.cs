using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.DataAccess
{
    public interface IRoutingConfigurationRepository
    {
        List<RoutingConfiguration> GetRoutingInfo();
    }

    public class RoutingConfigurationRepository : IRoutingConfigurationRepository
    {

        public List<RoutingConfiguration> GetRoutingInfo()
        {
            var routingInfoList = new List<RoutingConfiguration>();

            routingInfoList.Add(new RoutingConfiguration() { MessageAssembly = "Messages", MessageType = "Messages.CreateUser", DestinationEndpoint = "CreateUserHandler", DestinationMachine = "localhost" });
            routingInfoList.Add(new RoutingConfiguration() { MessageAssembly = "Messages", MessageType = "Messages.CreateUser", DestinationEndpoint = "CreateUserHandler2", DestinationMachine = "localhost" });
            
            return routingInfoList;
        }
    }


   
}
