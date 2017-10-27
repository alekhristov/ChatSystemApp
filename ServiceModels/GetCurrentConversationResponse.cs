using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class GetCurrentConversationResponse
    {
        [DataMember]
        public List<MessageDTO> Messages { get; set; }
    }
}
