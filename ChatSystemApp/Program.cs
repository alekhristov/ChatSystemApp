using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Alek.ChatService.ChatServiceReference;

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
        private static IList<Guid> messagesGuid;


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
                Console.WriteLine("You are connected to Alek's ChatSystem!\r\nPress 1 to Send message:\r\nPress 2 to Get chat history:\r\nPress 3 to Disconnect:");
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

                                if (messages == null)
                                {
                                    messages = new List<string>();
                                }

                                messages.Add(messageRequest.Message);
                                client.SendMessage(messageRequest);
                                break;

                            case CommonCommands.getChatHistory:
                                break;

                            case CommonCommands.disconnect:

                                var disconnectRequest = new DisconnectRequest();
                                client.Disconnect(disconnectRequest);
                                client = null;
                                Console.WriteLine("You have been disconnected.");
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
                    messagesGuid = new List<Guid>();
                }

                if (messages.Count != responseCurrentConversation.Messages.Count())
                {
                    //var firstNewMessage = response.Messages.Count() - (response.Messages.Count() - messages.Count);

                    //for (int i = firstNewMessage; i < response.Messages.Count(); i++)
                    //{
                    //    messages.Add($"{UserName}: {response.Messages[i]}");
                    //    Console.WriteLine(messages[i]);
                    //}

                    foreach (var message in responseCurrentConversation.Messages.OrderBy(m => m.SentTime))
                    {
                        if (!messagesGuid.Contains(message.Guid))
                        {
                            messagesGuid.Add(message.Guid);
                            messages.Add($"{message.Message}");
                            Console.WriteLine();
                            Console.WriteLine($"{message.Sender}: {message.Message}");
                        }
                    }

                    //var lastMessage = responseCurrentConversation.Messages.Last();
                    //messages.Add(lastMessage.Message);

                    //Console.WriteLine($"{lastMessage.Sender}: {lastMessage.Message}");
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
