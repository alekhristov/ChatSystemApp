using System;
using System.Collections.Generic;

namespace Alek.ChatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChatService" in both code and config file together.
    public class ChatService : IChatService
    {
        private Dictionary<Guid, string> connectedUsers = new Dictionary<Guid, string>();
        private IList<MessageDTO> currentConversation = null;

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

                var user = new UserDTO()
                {
                    Username = request.Username
                };

                using (ChatSystemAppDBContext dbContext = new ChatSystemAppDBContext())
                {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }

                return new ConnectResponse()
                {
                    IsConnected = true,
                    UserGuid = guid,
                    Username = request.Username
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DisconnectResponse Disconnect(DisconnectRequest request)
        {
            foreach (var user in connectedUsers)
            {
                if (user.Key == request.UserGuid)
                {
                    connectedUsers.Remove(user.Key);
                }
            }

            //save currentConversation in DB.
            currentConversation = null;

            return new DisconnectResponse();
        }

        public GetChatHistoryResponse GetChatHistory(GetChatHistoryRequest request)
        {
            throw new NotImplementedException();
        }

        public GetOnlineUsersResponse GetOnlineUsers(GetOnlineUsersRequest request)
        {
            throw new NotImplementedException();
        }

        public SendMessageResponse SendMessage(SendMessageRequest request)
        {
            if (connectedUsers.Count == 2)
            {
                var message = new MessageDTO();
                message.Message = request.Message;

                currentConversation.Add(message);
            }
            else if (connectedUsers.Count == 1)
            {
                throw new Exception("No other users are connected to the server!");
            }

            return new SendMessageResponse();
        }
    }
}
