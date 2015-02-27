
namespace CreateUserHandler
{
    using DynamicRouting;
    using NServiceBus;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.ScaleOut().UseSingleBrokerQueue();
            configuration.Pipeline.Register<DynamicRouting.DynamicRoutingStepInPipeline>();
            configuration.CustomConfigurationSource(new DynamicRoutingConfiguration());

        }
    }
}
