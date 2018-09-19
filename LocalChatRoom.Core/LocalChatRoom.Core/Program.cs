using LocalChatRoom.Core.ChatRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChatRoom.Core
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1) return -1;
            if (Directory.Exists(args[0]) == false) return -2;
            IChatRepository repository = new ChatRepository(args[0]);
            Console.WriteLine("StartChat");
            repository.OnRecivedNewMessage(s =>
            {
                if (string.IsNullOrEmpty(s)) return;
                Console.WriteLine(s);
            });
            repository.StartChat();

            while (true)
            {
                var mess = Console.ReadLine();
                if (mess.Equals("q")) break;
                Task.Run(() => repository.SendMessage(mess));
            }

            repository.EndChat();
            return 0;
        }
    }
}
