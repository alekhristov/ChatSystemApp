using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alek.ChatService
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ChatSystemAppDBContext();

            var userNames = dbContext.Users.Select(u => u.Username);

            Console.WriteLine(string.Join(", ", userNames));
            Console.ReadLine();
        }
    }
}
