using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Persistence;
using NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions;

namespace DynamicRouting
{
    public class DynamicRoutingSubscriptionStorage : ISubscriptionStorage
    {
        public IEnumerable<NServiceBus.Address> GetSubscriberAddressesForMessage(IEnumerable<NServiceBus.Unicast.Subscriptions.MessageType> messageTypes)
        {
           // throw new NotImplementedException();

            var addresses = new List<Address>();

            addresses.Add(new Address("UserCreatedHandler", "localhost"));
            
            return addresses;
        }

        public void Init()
        {
            //throw new NotImplementedException();
        }

        public void Subscribe(NServiceBus.Address client, IEnumerable<NServiceBus.Unicast.Subscriptions.MessageType> messageTypes)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(NServiceBus.Address client, IEnumerable<NServiceBus.Unicast.Subscriptions.MessageType> messageTypes)
        {
            throw new NotImplementedException();
        }
    }

    public class DynamicRoutingInMemoryPersistence : PersistenceDefinition
    {
        public DynamicRoutingInMemoryPersistence()
        {
            Supports<StorageType.Subscriptions>(s => s.EnableFeatureByDefault<DynamicRoutingInMemoryPersistenceFeature>());
            Supports<StorageType.Timeouts>(s => s.EnableFeatureByDefault<InMemoryTimeoutPersistence>());
            Supports<StorageType.Sagas>(s => s.EnableFeatureByDefault<InMemorySagaPersistence>());
        }
    }

    public class DynamicRoutingInMemoryPersistenceFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Container.ConfigureComponent<DynamicRoutingSubscriptionStorage>(DependencyLifecycle.SingleInstance);
        }
    }
}
