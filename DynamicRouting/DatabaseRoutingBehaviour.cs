using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseRouting.DataAccess;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast;

namespace DatabaseRouting
{
    public class DatabaseRoutingBehaviour: IBehavior<OutgoingContext>
    {
        public IBus Bus { get; set; }

        private ICommandRoutingConfigurationRepository routingConfigurationRepository;

        public DatabaseRoutingBehaviour()
        {
            this.routingConfigurationRepository = new CommandRoutingConfigurationRepository();
        }

        public void Invoke(OutgoingContext context, Action next)
        {
            //won't process publishes
            if (context.DeliveryOptions is PublishOptions)
            {
                next();

                return;

            }

            //won't process reply options
            if (context.DeliveryOptions is ReplyOptions)
            {
                next();

                return;

            }


            var routingInfo = routingConfigurationRepository.GetRoutingInfo();

            List<CommandRoutingConfiguration> possibleEndpoints=null;

            //sendonly endpoint, we don´t filter by sources
            if (Bus==null || Bus.CurrentMessageContext == null || (Bus.CurrentMessageContext != null && Bus.CurrentMessageContext.ReplyToAddress == null))
            {
                possibleEndpoints = routingConfigurationRepository.FindEndpointsBy(
                                                       context.OutgoingLogicalMessage.MessageType.ToString(),
                                                       context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name);
            }
            else          
                //not send only
                if (Bus != null && Bus is UnicastBus && ((UnicastBus)Bus).InputAddress != null)
                {
                    //look if there is a concrete endpoint for that component

                    var inputAddress = ((UnicastBus)Bus).InputAddress;

                    possibleEndpoints = routingConfigurationRepository.FindEndpointsBy(                                                          
                                                         context.OutgoingLogicalMessage.MessageType.ToString(),
                                                         context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name,                                                         
                                                         inputAddress.Queue);

                    //if not look for default endpoints
                    if (possibleEndpoints == null || possibleEndpoints.Count == 0)
                    {
                        possibleEndpoints = routingConfigurationRepository.FindEndpointsBy(
                                                         context.OutgoingLogicalMessage.MessageType.ToString(),
                                                         context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name);
                      
                    }
                }
            

            CommandRoutingConfiguration finalEndpoint = null;


            if (possibleEndpoints==null || possibleEndpoints.Count() == 0)
            {
                throw new Exception("NO ENDPOINT FOUND FOR " + context.OutgoingLogicalMessage.MessageType.ToString());
            }
            //random with the endpoints
            if (possibleEndpoints.Count() > 1)
            {
                Random rnd = new Random();
                int selected = rnd.Next(0, possibleEndpoints.Count());

                finalEndpoint = possibleEndpoints[selected];
            }
            if (possibleEndpoints.Count() == 1)
            {
                finalEndpoint = possibleEndpoints[0];
            }

           
            //change message addresss.
            ((NServiceBus.Unicast.SendOptions)(context.DeliveryOptions)).Destination = new Address(finalEndpoint.DestinationEndpoint, finalEndpoint.DestinationMachine);

                                                                             
            next();
        }

    

      
    }

    public class DatabaseRoutingStepInPipeline : RegisterStep
    {
        public DatabaseRoutingStepInPipeline()
            : base("NewStepInPipeline", typeof(DatabaseRoutingBehaviour), "Looks for an endpoint in the database")
        {

            InsertBefore(WellKnownStep.DispatchMessageToTransport);
        }
    }

}
