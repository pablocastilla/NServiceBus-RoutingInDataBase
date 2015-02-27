using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace UserCreatedHandler
{
    public class UserCreatedHandler : IHandleMessages<UserCreated>
    {
        public void Handle(UserCreated message)
        {
           
        }
    }
}
