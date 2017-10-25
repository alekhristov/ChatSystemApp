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
        private static  IList<String> messages;

        static void Main(string[] args)
        {
            Console.Write("Please enter your username: ");
            var name = Console.ReadLine();

            var request = new ConnectRequest();
            request.Username = name;
            UserName = name;

            var response = client.Connect(request);

            if (response.IsConnected)
            {
                Console.WriteLine("You are connected to Alek's ChatSystem.");
                Task.Factory.StartNew( async () => {
                    while(true)
                    {
                        PullMsgs();
                        await Task.Delay(1000);
                    }
                });

                while (true)
                { 
                    Console.Write("Command: ");

                    CommonCommands command;
                    var result = Enum.TryParse<CommonCommands>(Console.ReadLine(), out command);
                    if (result)
                    {
                        switch (command)
                        {
                            case CommonCommands.sendMessage:
                                Console.Write("Send:");

                                var messageRequest = new SendMessageRequest();
                                messageRequest.Message = Console.ReadLine();
                                messageRequest.SenderGuid = response.UserGuid;

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
                                client.Disconnect(new DisconnectRequest());
                                client = null;
                                Console.Write("You have been disconnected");
                                return;
                        }
                    }
                }
            }
        }

        private static void PullMsgs()
        {
            var request = new GetChatHistoryRequest();
            request.User1 = UserName;
            request.CurrentTime = DateTime.Now;

            var response = client.GetChatHistory(request);
            if (response != null && response.Messages.Any())
            {
                if (messages == null)
                {
                    messages = new List<string>();
                }

                if (messages.Count != response.Messages.Count())
                {
                    var lastMessage = response.Messages.Last();
                    messages.Add(lastMessage.Message);
                    Console.WriteLine(lastMessage.Message);
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
