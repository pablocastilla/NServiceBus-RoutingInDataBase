using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace CreateUserHandler
{
    public class CreateUserHandler:IHandleMessages<CreateUser>
    {
        public void Handle(CreateUser message)
        {
           
        }
    }
}
