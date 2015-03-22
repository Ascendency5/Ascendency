using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
    /// Interaction logic for OnlineLobbyUserControl.xaml
    /// </summary>
    public partial class OnlineLobbyUserControl : UserControl
    {
        public OnlineLobbyUserControl()
        {
            InitializeComponent();
            Networkmanager.OnDiscovery += on_peer_discovery;
            Networkmanager.OnDisconnect += on_peer_disconnect;
            
            List<ListBoxItem> items = new List<ListBoxItem>();
            foreach (KulamiPeer peer in PeerHolder.Peers)
            {
                if(peer.Name != null)
                    items.Add(new ListBoxItem() {Content = peer.Name});
            }

            OnlinePlayersListBox.ItemsSource = items;
        }

        private void on_peer_disconnect(object sender, EventArgs e)
        {

        }

        private void on_peer_discovery(object sender, EventArgs e)
        {
            List<ListBoxItem> items = (List<ListBoxItem>) OnlinePlayersListBox.ItemsSource;
            string name = ((KulamiPeer) sender).Name;
            items.Add(new ListBoxItem() {Content = name});
            OnlinePlayersListBox.ItemsSource = items;
        }

        private void CancelIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Networkmanager.OnDiscovery -= on_peer_discovery;
            Networkmanager.OnDisconnect -= on_peer_disconnect;

            //animate the cancel button from the ExitControl, then kill anim object
            UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            ContentControlActions.FadeOut();
        }

        private void UserControlButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == ChallengeIdle)
                UserControlAnimation.FadeInUserControlButton(ChallengeHover, false);
            else if (sender == ConnectIdle)
                UserControlAnimation.FadeInUserControlButton(ConnectHover, false);
            else if (sender == CancelIdle)
                UserControlAnimation.FadeInUserControlButton(CancelHover, false);

            //added sound effect for the button
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Resources/Audio/UserControlButtonHover.wav");
            player.Play();
        }

        private void UserControlButton_MouseEnter(object sender, MouseEventArgs e)
        {
            UserControlAnimation.FadeInUserControlButton(sender, false);
        }

        private void UserControlButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ChallengeHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(ChallengeHover, true);
            if (ConnectHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(ConnectHover, true);
            if (CancelHover.Opacity < 1)
                UserControlAnimation.FadeInUserControlButton(CancelHover, true);

            UserControlAnimation.FadeInUserControlButton(sender, true);
        }
    }
}
