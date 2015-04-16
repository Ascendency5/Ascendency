using System;
using System.Windows.Controls;
using System.Windows.Input;
using Ascendancy.Networking;

namespace Ascendancy.User_Controls.Multiplayer
{
    /// <summary>
    /// Interaction logic for NetworkGameDisconnectUserControl.xaml
    /// </summary>
    public partial class NetworkGameDisconnectUserControl : UserControl
    {
        private readonly VoidFunctionTemplate.Function Callback;
        private readonly KulamiPeer Peer;
        public NetworkGameDisconnectUserControl(KulamiPeer peer, VoidFunctionTemplate.Function callback)
        {
            InitializeComponent();
            Peer = peer;
            Peer.OnConnectionChange += on_connect_change;
            Callback = callback;
        }

        private void on_connect_change(object sender, NetConnectionEventArgs e)
        {
            if (!e.Connected) return;

            Peer.OnConnectionChange = null;
            ContentControlActions.FadeOut();
        }

        private void MainMenu_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentControlActions.FadeOut();
            Callback();
        }
    }
}
