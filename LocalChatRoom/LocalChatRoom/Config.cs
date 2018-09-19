using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChatRoom
{
    class Config
    {
        public static string UserName => ConfigurationManager.AppSettings["user_name"];
        public static string ChatDirectoryPath => ConfigurationManager.AppSettings["chat_directory_path"];
    }
}
