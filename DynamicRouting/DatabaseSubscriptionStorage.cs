using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseRouting.DataAccess;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Persistence;
using NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions;

namespace DatabaseRouting
{
    public class DatabaseRoutingSubscriptionStorage : ISubscriptionStorage
    {
        public IEnumerable<NServiceBus.Address> GetSubscriberAddressesForMessage(IEnumerable<NServiceBus.Unicast.Subscriptions.MessageType> messageTypes)
        {
   
            var addresses = new List<Address>();
                     

            IEventSubscriptionRepository rep = new EventSubscriptionRepository();

            var subs = rep.GetSubscriptionByTypes(messageTypes);


            //if there are subscribers with the same endpoint a random is done between them. 
            var results = from s in subs
                          group s by s.SubscriberEndpoint into g
                          select new { Endpoint = g.Key, Subscriptions = g.ToList() };

            foreach (var r in results)
            { 
                if (r.Subscriptions.Count() > 1)
                {
                    Random rnd = new Random();
                    int selected = rnd.Next(0, subs.Count());

                    addresses.Add(new Address(r.Subscriptions[selected].SubscriberEndpoint,r.Subscriptions[selected].SubscriberMachine));
                }
                if (subs.Count() == 1)
                {
                    addresses.Add(new Address(r.Subscriptions[0].SubscriberEndpoint, r.Subscriptions[0].SubscriberMachine));
                }
            }

                      
            return addresses;
        }

        public void Init()
        {
            
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

    public class DatabaseRoutingInMemoryPersistence : PersistenceDefinition
    {
        public DatabaseRoutingInMemoryPersistence()
        {
            Supports<StorageType.Subscriptions>(s => s.EnableFeatureByDefault<DatabaseRoutingInMemoryPersistenceFeature>());
            Supports<StorageType.Timeouts>(s => s.EnableFeatureByDefault<InMemoryTimeoutPersistence>());
            Supports<StorageType.Sagas>(s => s.EnableFeatureByDefault<InMemorySagaPersistence>());
        }
    }

    public class DatabaseRoutingInMemoryPersistenceFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Container.ConfigureComponent<DatabaseRoutingSubscriptionStorage>(DependencyLifecycle.SingleInstance);
        }
    }
}
