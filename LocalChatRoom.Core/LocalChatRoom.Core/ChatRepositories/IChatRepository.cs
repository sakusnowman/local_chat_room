using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChatRoom.Core.ChatRepositories
{
    public interface IChatRepository
    {
        void StartChat();
        void EndChat();
        Task SendMessage(string message);
        void OnRecivedNewMessage(Action<string> action);
    }
}
