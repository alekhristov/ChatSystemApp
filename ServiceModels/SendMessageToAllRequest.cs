using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class SendMessageRequest
    {
        [DataMember]
        public int SenderID { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string RecipientUsername { get; set; }
    }
}