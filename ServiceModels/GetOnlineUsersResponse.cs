using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class GetOnlineUsersResponse
    {
        [DataMember]
        public List<string> OnlineUsers { get; set; }
    }
}