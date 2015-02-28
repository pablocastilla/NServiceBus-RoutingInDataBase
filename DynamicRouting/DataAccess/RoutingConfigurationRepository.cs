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

        List<RoutingConfiguration> FindEndpointsBy(string messageType, string messageAssembly, string sourceMachine = null, string sourceEndpoint = null);
    }

    public class RoutingConfigurationRepository : IRoutingConfigurationRepository
    {
        List<RoutingConfiguration> routingInfoList ;

        public RoutingConfigurationRepository()
        {
            routingInfoList = new List<RoutingConfiguration>();

            routingInfoList.Add(new RoutingConfiguration() { MessageAssembly = "Messages", MessageType = "Messages.CreateUser", DestinationEndpoint = "CreateUserHandler", DestinationMachine = "localhost" });
            routingInfoList.Add(new RoutingConfiguration() { MessageAssembly = "Messages", MessageType = "Messages.CreateUser", DestinationEndpoint = "CreateUserHandler2", DestinationMachine = "localhost" });
        
        }

        public List<RoutingConfiguration> GetRoutingInfo()
        {
            
            return routingInfoList;
        }

        public List<RoutingConfiguration> FindEndpointsBy(string messageType, string messageAssembly, string sourceMachine = null, string sourceEndpoint = null)
        {

            var possibleEndpoints = routingInfoList.Where(r =>
                                                    r.MessageType == messageType
                                                    && r.MessageAssembly == messageAssembly
                                                );

            if (string.IsNullOrEmpty(sourceMachine))
                possibleEndpoints = possibleEndpoints.Where(r => r.SourceMachine == sourceMachine);

            if (string.IsNullOrEmpty(sourceEndpoint))
                possibleEndpoints = possibleEndpoints.Where(r => r.SourceEndpoint == sourceEndpoint);


            return possibleEndpoints.ToList();
        }
    }


   
}
