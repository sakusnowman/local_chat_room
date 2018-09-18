using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LocalChatRoom.Core
{
    class ChatRepository
    {
        bool isObserve = false;
        bool failedToRead = false;
        string changedFilePath = "";
        List<Action<string>> actions = new List<Action<string>>();

        public void AddActionWhenRepositoryChanged(Action<string> action)
        {
            actions.Add(action);
        }

        public void StartObserve(string directoryPath)
        {
            if (isObserve) return;
            isObserve = true;
            failedToRead = false;
            FileSystemWatcher watcher = new FileSystemWatcher(directoryPath);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.chat";
            watcher.Changed += new FileSystemEventHandler(async (object sender, FileSystemEventArgs e) =>
            {
                changedFilePath = e.FullPath;
                try
                {
                    await Task.Delay(500);
                    ActionAll(changedFilePath);
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
            while (isObserve)
            {
                await Task.Delay(5000);
                if (failedToRead == false) continue;
                try
                {
                    ActionAll(changedFilePath);
                }
                catch (Exception e)
                {
                    return;
                }
                failedToRead = false;
            }
        }

        string FileFileter
        {
            get
            {
                var today = DateTime.Today;
                var result ="" + today.Year + today.Month + today.Day + ".chat";
                return result;
            }
        }

        void ActionAll(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var value = reader.ReadToEnd();
                reader.Close();
                actions.ForEach(a => a(value));
                reader.Close();
            }
        }

        public void StopObserve() => isObserve = false;
    }
}
