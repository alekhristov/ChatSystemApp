using System;
using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class UserConversationHistoryModel
    {
        [DataMember]
        public string OtherParticipantUserName { get; set; }

        [DataMember]
        public DateTime StartDataTime { get; set; }

        [DataMember]
        public DateTime EndDateTime { get; set; }
    }
}
