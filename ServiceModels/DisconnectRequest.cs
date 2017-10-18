using System.Runtime.Serialization;

namespace Alek.ChatService
{
    [DataContract]
    public class DisconnectRequest
    {
        [DataMember]
        public int UserID { get; set; }
    }
}