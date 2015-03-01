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
        static int i = 1;
        public IBus Bus { get; set; }
        public void Handle(CreateUser message)
        {
            Console.WriteLine("Message recieved in CreateUserHandler " + i);
            i++;

            Bus.Publish(new UserCreated());

            Bus.Reply(new CreateUserResponse());
           // Bus.Send(message);
        }
    }
}
