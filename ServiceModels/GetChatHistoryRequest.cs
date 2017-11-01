using System;
using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class GetChatHistoryRequest
    {
        [DataMember]
        public string User1 { get; set; }

        [DataMember]
        public string User2 { get; set; }

        [DataMember]
        public Guid UserGuid { get; set; }

        [DataMember]
        public DateTime CurrentTime { get; set; }
    }
}