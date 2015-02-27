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
       

        public void Invoke(OutgoingContext context, Action next)
        {
            IRoutingConfigurationRepository rep = new RoutingConfigurationRepository();

            var routingInfo = rep.GetRoutingInfo();

            var possibleEndpoints = routingInfo.Where(r => 
                                    r.MessageType == context.OutgoingLogicalMessage.MessageType.ToString()
                                    && r.MessageAssembly == context.OutgoingLogicalMessage.MessageType.Assembly.GetName().Name
                                    ).ToList();

            RoutingConfiguration finalEndpoint = null;

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

            if (possibleEndpoints.Count() == 0)
            {
                throw new Exception("NO ENDPOINT FOUND FOR " + context.OutgoingLogicalMessage.MessageType.ToString());
            }

            ((NServiceBus.Unicast.SendOptions)(context.DeliveryOptions)).Destination = new Address(finalEndpoint.DestinationEndpoint, finalEndpoint.DestinationMachine);

                                                                             
            next();
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

}
