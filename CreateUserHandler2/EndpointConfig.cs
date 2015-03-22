
namespace CreateUserHandler2
{
    using DatabaseRouting;
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

            configuration.UsePersistence<DatabaseRoutingInMemoryPersistence>();
            configuration.DisableFeature<AutoSubscribe>();
            configuration.Pipeline.Register<DatabaseRouting.DatabaseRoutingStepInPipeline>();
            configuration.CustomConfigurationSource(new DatabaseRoutingConfiguration());

        }
    }
}
