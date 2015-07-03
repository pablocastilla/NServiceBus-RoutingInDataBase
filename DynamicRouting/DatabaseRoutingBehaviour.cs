using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

            if (context.DeliveryOptions is SendOptions)
            {
                var so = context.DeliveryOptions as SendOptions;     
          
                string isSagaTimeOutMessage="";

                if(context.OutgoingMessage.Headers.TryGetValue("NServiceBus.IsSagaTimeoutMessage", out isSagaTimeOutMessage))
                {
                    if(Convert.ToBoolean(isSagaTimeOutMessage))
                    {
                        next();

                        return;
                    }
                }              

              
            }


            var routingInfo = routingConfigurationRepository.GetRoutingInfo();

            List<CommandRoutingConfiguration> possibleEndpoints=null;


            string sourceEndpoint = ResolveSourceEndpoint();


            if (!string.IsNullOrEmpty(sourceEndpoint))
            {
                //look if there is a concrete endpoint for that component
                possibleEndpoints = routingConfigurationRepository.FindEndpointsBy(
                                                        context.OutgoingLogicalMessage.MessageType.ToString(),
                                                        context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name,
                                                        sourceEndpoint);
            }


            ///if we don't find any endpoint for that source we look for default endpoints
            if (possibleEndpoints == null || possibleEndpoints.Count == 0)
            {
                possibleEndpoints = routingConfigurationRepository.FindEndpointsBy(
                                                    context.OutgoingLogicalMessage.MessageType.ToString(),
                                                    context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name);
                      
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


        //resolves source endpoint and returns null if not found. Get's the queue name or the 
        private string ResolveSourceEndpoint()
        {
            string inputAddress = null;

            //wcf send only context
            if (OperationContext.Current != null)
            {
                inputAddress = OperationContext.Current.Host.Description.ConfigurationName;
            }
            else if (Bus != null && Bus is UnicastBus && ((UnicastBus)Bus).InputAddress != null)
            {
                //look if there is a concrete endpoint for that component
                //THIS MUST BE CHANGED TO THE REAL WINDOWS SERVICE NAME
                inputAddress = ((UnicastBus)Bus).InputAddress.Queue;
            }


            return inputAddress;
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
