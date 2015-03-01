using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Saga;

namespace Messages
{
    public class NewUser : ICommand
    {
       
        public string Name { get; set; }
    }
}
