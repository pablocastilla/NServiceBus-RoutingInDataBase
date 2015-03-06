using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicRouting.DataAccess;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace DynamicRouting
{
    public class DynamicRoutingConfiguration : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            // the part you are overriding
            if (typeof(T) == typeof(UnicastBusConfig))
            {
                var endpointMappint = new MessageEndpointMappingCollection();

                ICommandRoutingConfigurationRepository rep = new CommandRoutingConfigurationRepository();

                var routingInfo = rep.GetRoutingInfo();
                var messagesAlreadyInConfiguration = new List<string>();
                

                foreach (var ep in routingInfo)
                {
                    if (messagesAlreadyInConfiguration.Contains(ep.MessageType))
                    {
                        continue;
                    }
                    
                    endpointMappint.Add(new MessageEndpointMapping() 
                                        {
                                            AssemblyName=ep.MessageAssembly,
                                            TypeFullName=ep.MessageType,
                                            Endpoint = ep.DestinationEndpoint+"@"+ep.DestinationMachine
                                        });

                    messagesAlreadyInConfiguration.Add(ep.MessageType);
                }

                return new UnicastBusConfig
                {
                    MessageEndpointMappings = endpointMappint
                } as T;
            }
            // leaving the rest of the configuration as is:
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
}
