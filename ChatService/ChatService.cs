using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Alek.ChatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChatService" in both code and config file together.
    public class ChatService : IChatService
    {
        private static Dictionary<Guid, string> connectedUsers = new Dictionary<Guid, string>();
        private static IList<MessageDTO> currentConversation = new List<MessageDTO>();
        private static ConversationDTO conversationDTO = new ConversationDTO();
        private static Users_ConversationsDTO usersConversationsDTO = new Users_ConversationsDTO();

        public ConnectResponse Connect(ConnectRequest request)
        {
            try
            {
                if (connectedUsers.Count == 2)
                {
                    throw new Exception("You can't access the server, there is already two users connected to it!");
                }

                var guid = Guid.NewGuid();
                connectedUsers.Add(guid, request.Username);

                if (connectedUsers.Count == 2)
                {
                    currentConversation = new List<MessageDTO>();
                }

                var userDTO = new UserDTO()
                {
                    Username = request.Username
                };

                conversationDTO.StartTime = DateTime.UtcNow;

                var user = MapDTOToEntity.ConvertUserDTOToUser(userDTO);

                using (ChatSystemAppDBContext dbContext = new ChatSystemAppDBContext())
                {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }
                usersConversationsDTO.UserID = user.UserID;

                return new ConnectResponse()
                {
                    IsConnected = true,
                    UserGuid = guid,
                    Username = request.Username,
                    SenderUserID = user.UserID
                };
            }
            catch (Exception ex)
            {
                return new ConnectResponse() { IsConnected = false };
            }
        }

        public DisconnectResponse Disconnect(DisconnectRequest request)
        {
            foreach (var user in connectedUsers)
            {
                if (user.Key == request.UserGuid)
                {
                    connectedUsers.Remove(user.Key);
                    break;
                }
            }
            conversationDTO.EndTime = DateTime.UtcNow;

            if (currentConversation != null)
            {
                //save currentConversation in DB.
                using (ChatSystemAppDBContext dbContext = new ChatSystemAppDBContext())
                {
                    var conversation = MapDTOToEntity.ConvertConversationDTOToConversation(conversationDTO);
                    dbContext.Conversations.Add(conversation);
                    dbContext.SaveChanges();

                    var messages = currentConversation.Select(c => MapDTOToEntity.ConvertMessageDToToMessage(c));
                    dbContext.Messages.AddRange(messages);
                    dbContext.SaveChanges();

                    //Add save in Users_Conversation
                    usersConversationsDTO.ConversationID = conversation.ConversationID;
                    var usersConversations = MapDTOToEntity.ConvertUserConversationsDTOToUsersConversations(usersConversationsDTO);
                    dbContext.Users_Conversations.Add(usersConversations);
                    dbContext.SaveChanges();
                }
            }

            currentConversation = null;

            return new DisconnectResponse();
        }

        public GetChatHistoryResponse GetChatHistory(GetChatHistoryRequest request)
        {
            var chatHistoryResponse = new GetChatHistoryResponse();

            if (connectedUsers.Count == 2)
            {
                using (var dbContext = new ChatSystemAppDBContext())
                {
                    var userNameIds = dbContext.Users
                        .Where(u => u.Username == request.User1)
                        .Select(u => u.UserID)
                        .ToList();

                    var usersConversationIds = dbContext.Users_Conversations
                        .Where(uc => userNameIds.Contains(uc.UserID))
                        .Select(uc => uc.ConversationID)
                        .ToList();

                    var dataLayerMessages = dbContext.Messages
                        .Where(m => usersConversationIds.Contains(m.ConversationID))
                        .Select(m => MapEntityToDTO.ConvertMessageToMessageDTO(m))
                        .OrderBy(m => m.SentTime)
                        .ToList();

                    chatHistoryResponse.Messages = dataLayerMessages;
                }
            }
            else
            {
                throw new ApplicationException("There is no other users connected to the application.");
            }

            return chatHistoryResponse;
        }

        public GetConversationHistoryResponse GetConversationHistory(GetConversationHistoryRequest request)
        {
            var getConversationHistoryResponse = new GetConversationHistoryResponse();

            using (var dbContext = new ChatSystemAppDBContext())
            {
                //1. Get usersIds by userName
                var userNameIds = dbContext.Users
                    .Where(u => u.Username == request.UserName)
                    .Select(u => u.UserID)
                    .ToList();

                //2. Get conversationIds from User_Conversations by userIds from 1.
                var conversationsIds = dbContext.Users_Conversations
                    .Where(uc => userNameIds.Contains(uc.UserID))
                    .Select(c => c.ConversationID)
                    .ToList();

                //3. Get Conversations by conversationIds from 2.
                var conversations = dbContext.Conversations
                    .Where(c => conversationsIds.Contains(c.ConversationID))
                    .Select(c => MapEntityToDTO.ConvertConversationToConversationDTO(c))
                    .ToList();

                //4. Create list of UserConversationHistoryModel
                // getConversationHistoryResponse.UserConversation 

            };
            return getConversationHistoryResponse;
        }

        public GetCurrentConversationResponse GetCurrentConversationHistory(GetCurrentConversationRequest request)
        {
            var currentConversationHistory = new GetCurrentConversationResponse();

            if (currentConversation != null)
            {
                currentConversationHistory.Messages = currentConversation.Where(m => m.SentTime < request.CurrentTime).OrderBy(m => m.SentTime).ToList();
            }

            return currentConversationHistory;
        }

        public GetOnlineUsersResponse GetOnlineUsers(GetOnlineUsersRequest request)
        {
            throw new NotImplementedException();
        }

        public SendMessageResponse SendMessage(SendMessageRequest request)
        {
            if (connectedUsers.Count == 2)
            {
                var messageDTO = new MessageDTO();
                messageDTO.Message = request.Message;
                messageDTO.SentTime = DateTime.UtcNow;
                messageDTO.Sender = request.SenderName;
                messageDTO.Guid = Guid.NewGuid();
                messageDTO.SenderUserID = request.SenderUserID;

                currentConversation.Add(messageDTO);
            }
            else if (connectedUsers.Count == 1)
            {
                throw new Exception("No other users are connected to the ChatSystem!");
            }

            return new SendMessageResponse();
        }
    }
}
