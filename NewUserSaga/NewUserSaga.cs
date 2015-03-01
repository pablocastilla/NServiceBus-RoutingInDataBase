using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Saga;

namespace NewUserSaga
{
    public class NewUserSaga : Saga<NewUserSagaData>, IAmStartedByMessages<NewUser>, IHandleMessages<CreateUserResponse>
    {
        
        public void Handle(NewUser message)
        {
            this.Data.Name = message.Name;

            Console.WriteLine("New User Saga started for "+this.Data.Name);
            Bus.Send(new CreateUser() { Name=message.Name});
        }

        public void Handle(CreateUserResponse message)
        {
            Console.WriteLine("reply received from createuserhandler " + this.Data.Name);

            this.MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<NewUserSagaData> mapper)
        {
           
        }
    }

    public class NewUserSagaData : ContainSagaData
    {
        [Unique]
        public string Name { get; set; }
    }
}
