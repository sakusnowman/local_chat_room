using System;
using System.IO;
using System.Threading.Tasks;

namespace LocalChatRoom.Core
{
    class Program
    {
        static string oldSentence ="";
        static int Main(string[] args)
        {
            if (args.Length != 1) return -1;
            if (Directory.Exists(args[0]) == false) return -2;
            ChatRepository chatRepository = new ChatRepository();
            chatRepository.StartObserve(args[0]);
            chatRepository.AddActionWhenRepositoryChanged(s =>
            {
                var diff = NewSentence(oldSentence, s);
                if (string.IsNullOrEmpty(diff)) return;
                Console.Write(diff);
            });

            while (true)
            {
                var mess = Console.ReadLine();
                if (mess.Equals("q")) break;
            }
            chatRepository.StopObserve();
            return 0;
        }

        static string NewSentence(string old, string newSentence)
        {
            if (old.Length >= newSentence.Length) return "";
            var result = newSentence.Remove(0, old.Length);
            oldSentence = newSentence;
            return result;

        }
    }
}
