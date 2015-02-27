using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Unicast.Subscriptions.MessageDrivenSubscriptions;

namespace DynamicRouting
{
    public class DynamicRoutingSubscriptionStorage : ISubscriptionStorage
    {
        public IEnumerable<NServiceBus.Address> GetSubscriberAddressesForMessage(IEnumerable<NServiceBus.Unicast.Subscriptions.MessageType> messageTypes)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
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
}
