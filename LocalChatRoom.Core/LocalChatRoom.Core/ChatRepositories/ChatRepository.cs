using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChatRoom.Core.ChatRepositories
{
    class ChatRepository : IChatRepository
    {
        readonly FileSystemWatcher watcher;
        bool failedToRead = false;
        string changedFilePath = "";
        Action<string> action;
        MessageRepository messageRepository = new MessageRepository();

        public ChatRepository(string directoryPath)
        {
            watcher = new FileSystemWatcher(directoryPath);
        }

        public void EndChat()
        {
            watcher.EnableRaisingEvents = false;
            failedToRead = false;
        }

        public void OnRecivedNewMessage(Action<string> action)
        {
            this.action = action;
        }

        public async Task SendMessage(string message)
        {
            var today = DateTime.Today;
            var fileName = "" + today.Year + today.Month + today.Day + ".chat";

            StreamWriter writer = new StreamWriter(fileName, true, Encoding.Default);
            await writer.WriteAsync(message);
            writer.Close();
        }

        public void StartChat()
        {
            if (watcher.EnableRaisingEvents) return;
            failedToRead = false;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.chat";
            watcher.Changed += new FileSystemEventHandler(async (object sender, FileSystemEventArgs e) =>
            {
                try
                {
                    changedFilePath = e.FullPath;
                    await Task.Delay(500);
                    Update();
                }
                catch (Exception exc)
                {
                    failedToRead = true;
                }
            });

            watcher.EnableRaisingEvents = true;
            Task.Run(CheckIsUpdatedCorrectory);
        }

        async Task CheckIsUpdatedCorrectory()
        {
            while (watcher.EnableRaisingEvents)
            {
                await Task.Delay(5000);
                if (failedToRead == false) continue;
                try
                {
                    Update();
                }
                catch (Exception e)
                {
                    return;
                }
                failedToRead = false;
            }
        }

        void Update()
        {
            using (var reader = new StreamReader(changedFilePath))
            {
                var value = reader.ReadToEnd();
                reader.Close();
                action(messageRepository.GetNewMessage(value));
            }
        }
    }
}
