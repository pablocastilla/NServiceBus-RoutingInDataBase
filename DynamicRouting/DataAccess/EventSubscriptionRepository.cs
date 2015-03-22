using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseRouting.DataAccess
{
    public interface IEventSubscriptionRepository
    {
        List<EventSubscription> GetSubscriptionByTypes(IEnumerable<NServiceBus.Unicast.Subscriptions.MessageType> messageTypes);

    }

    public class EventSubscriptionRepository : IEventSubscriptionRepository
    {
        public List<EventSubscription> GetSubscriptionByTypes(IEnumerable<NServiceBus.Unicast.Subscriptions.MessageType> messageTypes)
        {
            return new List<EventSubscription>() { 
                new EventSubscription() { SubscriberMachine = "localhost", SubscriberEndpoint = "UserCreatedHandler" } ,
                new EventSubscription() { SubscriberMachine = "localhost", SubscriberEndpoint = "UserCreatedHandler" }};
        }
    }
}
