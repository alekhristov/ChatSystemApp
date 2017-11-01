using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class GetConversationHistoryResponse
    {
        [DataMember]
        public IEnumerable<UserConversationHistoryModel> UserConversation { get; set; }
    }
}