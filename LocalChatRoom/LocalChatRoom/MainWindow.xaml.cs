using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocalChatRoom
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ChatRepository chatRepository;

        public MainWindow()
        {
            InitializeComponent();

            chatRepository = new ChatRepository(s =>
            {
                this.Dispatcher.Invoke((Action)(() => this._chatMessageText.Text += s + Environment.NewLine));
            }, Config.ChatDirectoryPath);
            chatRepository.StartChat("LocalChatRoom.Core.exe");
            chatRepository.SendMessage("<<  IN  >> " + Config.UserName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void _myMessageText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key.Equals(Key.M))
                {
                    SendMessage();
                }
            }
        }

        private void SendMessage()
        {
            chatRepository.SendMessage(Config.UserName + ">>" + _myMessageText.Text);
            this._myMessageText.Text = "";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            chatRepository.SendMessage("<<  OUT  >> " + Config.UserName);
            chatRepository.EndChat();
        }
    }
}
