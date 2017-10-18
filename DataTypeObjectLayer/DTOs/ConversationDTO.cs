using System;

namespace Alek.ChatService
{
    public class ConversationDTO
    {
        public int User1ID { get; set; }

        public int User2ID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
