using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class ConnectResponse
    {
        [DataMember]
        public bool IsConnected{ get; set; }

        [DataMember]
        public int UserID { get; set; }
    }
}