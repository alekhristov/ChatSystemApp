using System;

namespace Alek.ChatService
{
    public class MessageDTO
    {
        public string Message { get; set; }

        public int SenderUserID { get; set; }

        public DateTime SentTime { get; set; }
    }
}
