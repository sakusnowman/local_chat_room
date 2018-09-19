using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChatRoom
{
    class ChatRepository
    {
        Process process;
        private readonly Action<string> actionWhenRecivedNewMessage;
        private readonly string chatDirectoryPath;
        private string notSendMessage = "";
        public ChatRepository(Action<string> actionWhenRecivedNewMessage, string chatDirectoryPath)
        {
            this.actionWhenRecivedNewMessage = actionWhenRecivedNewMessage;
            this.chatDirectoryPath = chatDirectoryPath;
        }

        public void StartChat(string exePath)
        {
            process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.FileName = exePath;
            process.StartInfo.Arguments = chatDirectoryPath;
            process.OutputDataReceived += new DataReceivedEventHandler((object sender, DataReceivedEventArgs e) =>
            {
                actionWhenRecivedNewMessage(e.Data);
            });

            var a = process.Start();
            process.BeginOutputReadLine();
        }

        public void EndChat()
        {
            process.StandardInput.WriteLine("q");
        }

        public void SendMessage(string message)
        {
            var today = DateTime.Today;
            notSendMessage = message;
            try
            {
                WriteMessage();
            }
            catch { }
        }

        async Task SendMessageAgainIfNeed()
        {
            while (process.HasExited == false)
            {
                await Task.Delay(1000);
                try
                {
                    WriteMessage();
                }
                catch { }
            }
        }

        void WriteMessage()
        {
            if (string.IsNullOrEmpty(notSendMessage)) return;
            process.StandardInput.WriteLine(notSendMessage);
            return;
        }
    }
}
