using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicRouting.DataAccess
{
    public interface IRoutingInfoRepository
    {
        List<RoutingInfo> GetRoutingInfo();
    }

    public class RoutingInfoRepository : IRoutingInfoRepository
    {

        public List<RoutingInfo> GetRoutingInfo()
        {
            var routingInfoList = new List<RoutingInfo>();

            routingInfoList.Add(new RoutingInfo() { Assembly = "Messages", MessageType = "Messages.CreateUser", DestinationEndpoint = "CreateUserHandler", DestinationMachine = "localhost" });
            routingInfoList.Add(new RoutingInfo() { Assembly = "Messages", MessageType = "Messages.CreateUser", DestinationEndpoint = "CreateUserHandler2", DestinationMachine = "localhost" });

            return routingInfoList;
        }
    }


   
}
