
namespace CreateUserHandler2
{
    using DynamicRouting;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Persistence;
    using NServiceBus.Transports;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {

            configuration.UsePersistence<HardCodedPersistence>();
            configuration.DisableFeature<AutoSubscribe>();
            configuration.Pipeline.Register<DynamicRouting.DynamicRoutingStepInPipeline>();
            configuration.CustomConfigurationSource(new DynamicRoutingConfiguration());

        }
    }
}
