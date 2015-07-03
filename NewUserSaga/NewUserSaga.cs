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
    public class NewUserSaga : Saga<NewUserSagaData>, IAmStartedByMessages<NewUser>, 
        IHandleMessages<CreateUserResponse>,
        IHandleTimeouts<NewUser>
    {
        
        public void Handle(NewUser message)
        {          
            RequestTimeout<NewUser>(DateTime.Now.AddSeconds(10));
            this.Data.Name = message.Name;
          
                              
        }

        public void Handle(CreateUserResponse message)
        {
            Console.WriteLine("reply received from createuserhandler " + this.Data.Name);

           

            this.MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<NewUserSagaData> mapper)
        {
           
        }

        public void Timeout(NewUser message)
        {
            Console.WriteLine("New User Saga started for " + this.Data.Name);


            Bus.Send(new CreateUser() { Name = message.Name });
        }
    }

    public class NewUserSagaData : ContainSagaData
    {
        [Unique]
        public string Name { get; set; }
    }
}
