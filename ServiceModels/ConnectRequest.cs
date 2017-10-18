using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class ConnectRequest
    {
        [DataMember]
        public string Username { get; set; }
    }
}