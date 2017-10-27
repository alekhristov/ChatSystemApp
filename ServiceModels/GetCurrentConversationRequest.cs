using System;
using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class GetCurrentConversationRequest
    {
        [DataMember]
        public string SenderName { get; set; }

        [DataMember]
        public DateTime CurrentTime { get; set; }

        [DataMember]
        public string RecipientName { get; set; }
    }
}
