using System;

namespace Alek.ChatService
{
    public class GetConversationHistoryRequest
    {
        public string UserName { get; set; }

        public Guid UserGuid { get; set; }
    }
}