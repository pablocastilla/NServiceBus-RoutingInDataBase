using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace CreateUserHandler
{
   
    public class Sender : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            RunSamples();
        }

        private void RunSamples()
        {
            for (int i = 0; i < 1000; i++)
            {
                Bus.Send(new NewUser
                {
                    Name = "Paco "+Guid.NewGuid().ToString()
                });
            }
        }


        public void Stop()
        {

        }
    }
}
