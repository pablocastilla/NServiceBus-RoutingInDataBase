using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.DataAccess
{
    public interface ICommandRoutingConfigurationRepository
    {
        List<CommandRoutingConfiguration> GetRoutingInfo();

        List<CommandRoutingConfiguration> FindEndpointsBy(string messageType, string messageAssembly, string sourceMachine = null, string sourceEndpoint = null);
    }

    public class CommandRoutingConfigurationRepository : ICommandRoutingConfigurationRepository
    {
        List<CommandRoutingConfiguration> routingInfoList ;

        public CommandRoutingConfigurationRepository()
        {
            routingInfoList = new List<CommandRoutingConfiguration>();

            routingInfoList.Add(new CommandRoutingConfiguration() { MessageAssembly = "Messages", MessageType = "Messages.CreateUser", DestinationEndpoint = "CreateUserHandler", DestinationMachine = "localhost" });
            routingInfoList.Add(new CommandRoutingConfiguration() { MessageAssembly = "Messages", MessageType = "Messages.CreateUser", DestinationEndpoint = "CreateUserHandler2", DestinationMachine = "localhost" });
            routingInfoList.Add(new CommandRoutingConfiguration() { MessageAssembly = "Messages", MessageType = "Messages.NewUser", DestinationEndpoint = "NewUserSaga", DestinationMachine = "localhost" });
        
        }

        public List<CommandRoutingConfiguration> GetRoutingInfo()
        {
            
            return routingInfoList;
        }

        public List<CommandRoutingConfiguration> FindEndpointsBy(string messageType, string messageAssembly, string sourceMachine = null, string sourceEndpoint = null)
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
