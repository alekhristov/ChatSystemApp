using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alek.ChatService.ChatServiceReference;
using System.Threading;

namespace Alek.ChatService
{
    enum CommonCommands
    {
        sendMessage = 1,
        getChatHistory = 2,
        disconnect = 3,
    }

    class Program
    {
        private static ChatServiceClient client = new ChatServiceClient();
        private static string UserName;
        private static IList<string> messages;

        static void Main(string[] args)
        {
            Console.Write("Please enter your username: ");
            var name = Console.ReadLine();
            Console.WriteLine();

            var request = new ConnectRequest();
            request.Username = name;
            UserName = name;

            var response = client.Connect(request);

            if (response.IsConnected)
            {
                Console.WriteLine("You are now connected to Alek's ChatSystem!\r\nPress 1 to Send message:\r\nPress 2 to Get chat history:\r\nPress 3 to Disconnect:");
                Task.Factory.StartNew(async () =>
                {
                    while (true)
                    {
                        PullMsgs();
                        await Task.Delay(1000);
                    }
                });

                while (true)
                {
                    Console.WriteLine("Command: ");

                    CommonCommands command;
                    var result = Enum.TryParse<CommonCommands>(Console.ReadLine(), out command);
                    if (result)
                    {
                        switch (command)
                        {
                            case CommonCommands.sendMessage:
                                Console.Write("Send: ");

                                var messageRequest = new SendMessageRequest();
                                messageRequest.Message = Console.ReadLine();
                                messageRequest.SenderGuid = response.UserGuid;
                                messageRequest.SenderName = UserName;
                                messageRequest.SenderUserID = response.SenderUserID;

                                if (messages == null)
                                {
                                    messages = new List<string>();
                                }

                                messages.Add(messageRequest.Message);
                                client.SendMessage(messageRequest);
                                break;

                            case CommonCommands.getChatHistory:
                                var chatHistoryRequest = new GetChatHistoryRequest();
                                chatHistoryRequest.CurrentTime = DateTime.UtcNow;
                                chatHistoryRequest.User1 = UserName;
                               var history = client.GetChatHistory(chatHistoryRequest).Messages;

                                foreach (var message in history.Where(m => m.SentTime < DateTime.UtcNow))
                                {
                                    Console.WriteLine($"{message.Sender}: {message.Message}");
                                }
                                break;

                            case CommonCommands.disconnect:

                                var disconnectRequest = new DisconnectRequest();
                                disconnectRequest.UserGuid = response.UserGuid;
                                client.Disconnect(disconnectRequest);
                                client = null;
                                Console.WriteLine("You have been disconnected.");
                                Thread.Sleep(2000);
                                return;
                        }
                    }
                }
            }
        }

        private static void PullMsgs()
        {
            var requestCurrentConversation = new GetCurrentConversationRequest();
            requestCurrentConversation.SenderName = UserName;
            requestCurrentConversation.CurrentTime = DateTime.UtcNow;

            var responseCurrentConversation = client.GetCurrentConversationHistory(requestCurrentConversation);
            if (responseCurrentConversation != null && responseCurrentConversation.Messages.Any())
            {
                if (messages == null)
                {
                    messages = new List<string>();
                }

                var totalMessages = responseCurrentConversation.Messages.Count();
                var numberOfOldMessages = messages.Count();
                var numberOfNewMessages = totalMessages - numberOfOldMessages;
                var newMessages = responseCurrentConversation.Messages.Skip(numberOfOldMessages).Take(numberOfNewMessages);

                if (newMessages != null && newMessages.Any())
                {
                    foreach (var message in newMessages)
                    {
                        messages.Add(message.Message);

                        if (message.Sender != UserName && message.Message != string.Empty)
                        {
                            Console.WriteLine($"{message.Sender}: {message.Message}");
                        }
                    }
                }
            }
        }

        ~Program()
        {
            client.Disconnect(new DisconnectRequest());
            client = null;
        }
    }
}
