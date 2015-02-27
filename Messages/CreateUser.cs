using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class CreateUser : ICommand
    {
        public string Name { get; set; }
    }
}
