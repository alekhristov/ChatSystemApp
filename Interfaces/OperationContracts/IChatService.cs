using System.ServiceModel;

namespace Alek.ChatService
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IChatService
    {
        [OperationContract]
        ConnectResponse Connect(ConnectRequest request);

        [OperationContract]
        SendMessageResponse SendMessage(SendMessageRequest request);

        [OperationContract]
        GetOnlineUsersResponse GetOnlineUsers(GetOnlineUsersRequest request);

        [OperationContract]
        GetChatHistoryResponse GetChatHistory(GetChatHistoryRequest request);

        [OperationContract]
        DisconnectResponse Disconnect(DisconnectRequest request);

        [OperationContract]
        GetCurrentConversationResponse GetCurrentConversationHistory(GetCurrentConversationRequest request);

        [OperationContract]
        GetConversationHistoryResponse GetConversationHistory(GetConversationHistoryRequest request);
    }
}
