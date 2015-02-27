using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicRouting.DataAccess;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

namespace DynamicRouting
{
    public class DynamicRoutingBehaviour: IBehavior<OutgoingContext>
    {
        public IBus Bus { get; set; }

        public void Invoke(OutgoingContext context, Action next)
        {
            IRoutingConfigurationRepository rep = new RoutingConfigurationRepository();

            var routingInfo = rep.GetRoutingInfo();

            List<RoutingConfiguration> possibleEndpoints=null;

            //sendonly endpoint
            if (Bus==null || Bus.CurrentMessageContext == null || (Bus.CurrentMessageContext != null && Bus.CurrentMessageContext.ReplyToAddress == null))
            {
                possibleEndpoints = FindEndpoints(routingInfo,
                                                       context.OutgoingLogicalMessage.MessageType.ToString(),
                                                       context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name);
            }
            else          
                //not send only
                if (Bus.CurrentMessageContext != null 
                    && Bus.CurrentMessageContext.Headers[Headers.OriginatingMachine]!=null
                    && Bus.CurrentMessageContext.Headers[Headers.OriginatingEndpoint]!=null)
                {
                    //look if there is a concrete endpoint for that component
               

                    possibleEndpoints = FindEndpoints(routingInfo,                                                          
                                                         context.OutgoingLogicalMessage.MessageType.ToString(),
                                                         context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name,
                                                         Bus.CurrentMessageContext.Headers[Headers.OriginatingMachine],
                                                         Bus.CurrentMessageContext.Headers[Headers.OriginatingEndpoint]);

                    //if not look for general endpoints
                    if (possibleEndpoints == null || possibleEndpoints.Count == 0)
                    {
                        possibleEndpoints = FindEndpoints(routingInfo,
                                                         context.OutgoingLogicalMessage.MessageType.ToString(),
                                                         context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name);
                      
                    }
                }
            

            RoutingConfiguration finalEndpoint = null;


            if (possibleEndpoints==null || possibleEndpoints.Count() == 0)
            {
                throw new Exception("NO ENDPOINT FOUND FOR " + context.OutgoingLogicalMessage.MessageType.ToString());
            }

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

        private List<RoutingConfiguration> FindEndpoints(List<RoutingConfiguration> routingInfo,  string messageType, string messageAssembly, string sourceMachine=null, string sourceEndpoint=null)
        {
            
            var possibleEndpoints = routingInfo.Where(r =>
                                                
                                                 r.MessageType == messageType
                                                && r.MessageAssembly == messageAssembly
                                                );

            if(string.IsNullOrEmpty(sourceMachine))
                possibleEndpoints = possibleEndpoints.Where(r=> r.SourceMachine ==sourceMachine );

              if(string.IsNullOrEmpty(sourceEndpoint))
                possibleEndpoints = possibleEndpoints.Where(r=> r.SourceEndpoint ==sourceEndpoint );


              return possibleEndpoints.ToList();                                      
        }

      
    }

    public class DynamicRoutingStepInPipeline : RegisterStep
    {
        public DynamicRoutingStepInPipeline()
            : base("NewStepInPipeline", typeof(DynamicRoutingBehaviour), "Looks for an endpoint in the database")
        {

            InsertBefore(WellKnownStep.DispatchMessageToTransport);
        }
    }

}
