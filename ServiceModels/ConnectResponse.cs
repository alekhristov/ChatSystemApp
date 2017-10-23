using System;
using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class ConnectResponse
    {
        [DataMember]
        public bool IsConnected{ get; set; }

        [DataMember]
        public Guid UserGuid { get; set; }

        [DataMember]
        public string Username { get; set; }
    }
}