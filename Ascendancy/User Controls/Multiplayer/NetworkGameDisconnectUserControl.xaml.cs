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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ascendancy.Game_Engine;
using Ascendancy.Networking;

namespace Ascendancy.User_Controls
{
    /// <summary>
    /// Interaction logic for NetworkGameDisconnectUserControl.xaml
    /// </summary>
    public partial class NetworkGameDisconnectUserControl : UserControl
    {
        private NetworkGameDisconnectMenuButtonHandler callback;

        public delegate void NetworkGameDisconnectMenuButtonHandler(object sender, NetworkGameDisconnectMenuOptionEventArgs eventArgs);

        public NetworkGameDisconnectUserControl(KulamiPeer peer, NetworkGameDisconnectMenuButtonHandler callback)
        {
            this.callback = callback;
            InitializeComponent();
            UserControlAnimation.StartButtonGradientSpin(Buttons);

            LeftGameNotificationLabel.Content = peer.Name + " has left the game.";
            LeftGameNotificationLabelGlow.Content = peer.Name + " has left the game.";
        }

        public void EventFilter(object sender)
        {
            if (sender == BackToLobby)
            {
                callback(this, new NetworkGameDisconnectMenuOptionEventArgs(NetworkGameDisconnectMenuOption.BackToLobby));
            }
            else if (sender == MainMenu)
            {
                callback(this, new NetworkGameDisconnectMenuOptionEventArgs(NetworkGameDisconnectMenuOption.MainMenu));
            }
        }


        private void NetworkGameDisconnectButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonUpStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

            EventFilter(sender);
        }

        private void NetworkGameDisconnectButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sss = (Canvas)sender;
            Storyboard localStoryboard = App.Current.FindResource("ButtonDownStoryboard") as Storyboard;
            Storyboard.SetTarget(localStoryboard, sss.Children[1]);
            localStoryboard.Begin();

        }

        private void NetworkGameDisconnectButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], true);

            //added sound effect for the button
            VolumeManager.play(@"Resources/Audio/UserControlButtonHover.wav");
        }

        private void NetworkGameDisconnectButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //todo get mouse down working with this
            Canvas animateThisCanvas = (Canvas)sender;
            UserControlAnimation.FadeInUserControlButton(animateThisCanvas.Children[0], false);
        }
    }

    public class NetworkGameDisconnectMenuOptionEventArgs : EventArgs
    {
        public readonly NetworkGameDisconnectMenuOption Option;

        public NetworkGameDisconnectMenuOptionEventArgs(NetworkGameDisconnectMenuOption option)
        {
            this.Option = option;
        }
    }

    public enum NetworkGameDisconnectMenuOption
    {
        BackToLobby,
        MainMenu
    }
}
