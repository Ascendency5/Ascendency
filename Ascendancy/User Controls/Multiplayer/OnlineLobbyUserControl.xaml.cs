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
using Ascendancy.Game_Engine;
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
                    items.Add(new ListBoxItem() {Content = peer});
            }

            OnlinePlayersListBox.ItemsSource = items;
            OnlinePlayerChallengesListBox.ItemsSource = new List<ListBoxItem>();
        }

        private void on_peer_disconnect(object sender, EventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;

            // todo OnGameResponse
            peer.OnGameRequest -= on_game_request;

            Dispatcher.Invoke(() =>
            {
                RemovePeer(OnlinePlayersListBox, peer);
                RemovePeer(OnlinePlayerChallengesListBox, peer);
            });
        }

        private void RemovePeer(ListBox listBox, KulamiPeer peer)
        {
            List<ListBoxItem> items = (List<ListBoxItem>) listBox.ItemsSource;
            listBox.ItemsSource = items.Where(
                x => ((KulamiPeer) x.Content).Identifier != peer.Identifier
                ).ToList();
            listBox.Items.Refresh();
        }

        private void AddPeer(ListBox listBox, KulamiPeer peer)
        {
            List<ListBoxItem> items = (List<ListBoxItem>) listBox.ItemsSource;
            items.Add(new ListBoxItem() { Content = peer });
            listBox.ItemsSource = items;
            listBox.Items.Refresh();
        }

        private void on_game_request(object sender, NetGameRequestEventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;

            Dispatcher.Invoke(() =>
            {
                AddPeer(OnlinePlayerChallengesListBox, peer);
            });
        }

        private void on_peer_discovery(object sender, EventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;
            if (string.IsNullOrEmpty(peer.Name)) return;

            peer.OnGameRequest += on_game_request;

            Dispatcher.Invoke(() =>
            {
                AddPeer(OnlinePlayersListBox, peer);
            });
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
            UserControlAnimation.FadeInUserControlButton(sender, false);

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
            UserControlAnimation.FadeInUserControlButton(sender, true);
        }

        private void ChallengeIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            KulamiPeer peer = getSelectedPeer(OnlinePlayersListBox);
            if (peer == null) return;

            // todo make this random number
            peer.SendRequest(5, true);
        }

        private void ConnectIdle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            KulamiPeer peer = getSelectedPeer(OnlinePlayerChallengesListBox);
            if (peer == null) return;

            peer.SendResponse(true);
            // Set up game engine
            HumanPlayer humanPlayer = new HumanPlayer(GameBoardUserControl.human_move_handler);
            NetworkPlayer networkPlayer = new NetworkPlayer(peer);

            Board board = BoardSetup.board_team5;

            GameEngine engine = new GameEngine(board, humanPlayer, networkPlayer,
                true ? PieceType.Red : PieceType.Black
                );
            engine.OnPlayerMove += networkPlayer.on_player_move;

            ContentControlActions.setUpControl(new GameBoardUserControl(engine));
        }

        private KulamiPeer getSelectedPeer(ListBox listBox)
        {
            ListBoxItem boxItem = listBox.SelectedItem as ListBoxItem;
            
            // todo Make this return the only element if there's only one elemet
            if (boxItem == null) return null;

            KulamiPeer peer = boxItem.Content as KulamiPeer;

            return peer;
        }
    }
}
