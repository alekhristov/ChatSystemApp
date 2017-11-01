namespace Alek.ChatService
{
    public static class MapEntityToDTO
    {
        public static ConversationDTO ConvertConversationToConversationDTO(Conversation conversation)
        {
            return new ConversationDTO()
            {
                StartTime = conversation.StartTime,
                EndTime = conversation.EndTime
            };
        }

        public static MessageDTO ConvertMessageToMessageDTO(Message message)
        {
            return new MessageDTO()
            {
                SenderUserID = message.SenderUserID,
                Message = message.Message1,
                SentTime = message.SentTime
            };
        }

        public static UserDTO ConvertUserToUserDTO(User user)
        {
            return new UserDTO()
            {
                UserID = user.UserID,
                Username = user.Username
            };
        }
    }
}
