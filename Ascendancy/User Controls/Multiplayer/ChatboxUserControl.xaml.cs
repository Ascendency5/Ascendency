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
using Ascendancy.Networking;

namespace Ascendancy.User_Controls.Multiplayer
{
    /// <summary>
    /// Interaction logic for ChatboxUserControl.xaml
    /// </summary>
    public partial class ChatboxUserControl : UserControl
    {
        private readonly KulamiPeer peer;
        private readonly string humanPlayerName;


        public ChatboxUserControl(KulamiPeer peer)
        {
            InitializeComponent();
            this.peer = peer;
            this.humanPlayerName = Networkmanager.ClientName;
            peer.OnChatMessage += on_chat_message;
        }

        private void on_chat_message(object sender, NetChatEventArgs eventArgs)
        {
            string textToAppend = peer.Name + ":  " + eventArgs.Message;
            
            Paragraph newParagraph = new Paragraph(new Run(textToAppend)) { LineHeight = 1, Foreground = Brushes.Orange };
            ChatRichTextBoxFlowDocument.Blocks.Add(newParagraph);
            ChatRichTextBox.ScrollToEnd();
        }

        private void ChatTextInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                handleChatInput();
            }
        }
        
        private void handleChatInput()
        {
            if (!String.IsNullOrEmpty(ChatTextInput.Text))
            {
                //todo: put this back in when networking works again, else will crash cause I'm passing in a null peer
                peer.SendChat(ChatTextInput.Text);

                string textToAppend = humanPlayerName + ":  " + ChatTextInput.Text;

                Paragraph newParagraph = new Paragraph(new Run(textToAppend))
                {
                    LineHeight = 1,
                    Foreground = Brushes.Cyan
                };

                ChatRichTextBoxFlowDocument.Blocks.Add(newParagraph);

                ChatRichTextBox.ScrollToEnd();
                ChatTextInput.Clear();
            }
        }

        private void ChatboxUserControl1_LostFocus(object sender, RoutedEventArgs e)
        {
            //todo: decide how the chat box will appear on the game screen. 
            //todo: depending on that decision, this may require moving the user control, adjusting the z index, ect
            //todo: same for Got_Focus function below
        }

        private void ChatboxUserControl1_GotFocus(object sender, RoutedEventArgs e)
        {
            ChatTextInput.Focus();
        }

        private void ChatboxUserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            ChatTextInput.Focus();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            handleChatInput();
        }
    }
}