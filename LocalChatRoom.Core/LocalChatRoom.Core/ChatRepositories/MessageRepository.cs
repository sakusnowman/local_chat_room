using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChatRoom.Core.ChatRepositories
{
    class MessageRepository
    {
        string oldValue = "";

        public string GetNewMessage(string value)
        {
            if (oldValue.Length >= value.Length) return "";
            var result = value.Remove(0, oldValue.Length);
            oldValue = value;
            return result;
        }
    }
}
