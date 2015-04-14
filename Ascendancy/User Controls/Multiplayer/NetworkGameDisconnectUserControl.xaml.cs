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
            InitializeComponent();
            this.callback = callback;

            LeftGameNotificationLabel.Content = peer.Name + " has left the game.";
            LeftGameNotificationLabelGlow.Content = peer.Name + " has left the game.";
        }

        private void MainMenu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            callback(this, new NetworkGameDisconnectMenuOptionEventArgs(NetworkGameDisconnectMenuOption.MainMenu));
        }

        private void BackToLobby_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            callback(this, new NetworkGameDisconnectMenuOptionEventArgs(NetworkGameDisconnectMenuOption.BackToLobby));
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
