using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alek.ChatService
{
    public class MapDTOToEntity
    {
        public static Conversation ConvertConversationDTOToConversation(ConversationDTO conversationDTO)
        {
            return new Conversation()
            {
                StartTime = conversationDTO.StartTime,
                EndTime = conversationDTO.EndTime
            };
        }

        public static Message ConvertMessageDToToMessage(MessageDTO messageDTO)
        {
            return new Message()
            {
                SenderUserID = messageDTO.SenderUserID,
                Message1 = messageDTO.Message,
                SentTime = messageDTO.SentTime
            };
        }

        public static User ConvertUserDTOToUser(UserDTO userDTO)
        {
            return new User()
            {
                UserID = userDTO.UserID,
                Username = userDTO.Username
            };
        }
    }
}
