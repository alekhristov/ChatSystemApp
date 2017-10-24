using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alek.ChatService.AlekChatServiceReference;

namespace Alek.ChatService
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServiceClient client = new ChatServiceClient();
            Console.Write("Please enter your username: ");
            var name = Console.ReadLine();

            var request = new ConnectRequest();
            request.Username = name;

            var response = client.Connect(request);

            if (response.IsConnected)
            {
                Console.WriteLine("You are connected to Alek's ChatSystem.");

                while (true)
                { 
                    Console.Write("Send: ");
                    var messageRequest = new SendMessageRequest();
                    messageRequest.Message = Console.ReadLine();
                    messageRequest.SenderGuid = response.UserGuid;

                    client.SendMessage(messageRequest);
                }
            }
        }
    }
}
