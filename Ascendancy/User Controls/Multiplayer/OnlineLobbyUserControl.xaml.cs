using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            Networkmanager.InLobby = true;
            
            List<ListBoxItem> items = new List<ListBoxItem>();

            TraceHelper.WriteLine(PeerHolder.Peers.Count);
            foreach (KulamiPeer peer in PeerHolder.Peers)
            {
                subscribe(peer);
                if (peer.Name != null && peer.InLobby)
                {
                    items.Add(new ListBoxItem() {Content = peer});
                    TraceHelper.WriteLine("Added peer");
                }
                else if (peer.Name == null)
                {
                    TraceHelper.WriteLine("Peer name is null");
                }
                else if (!peer.InLobby)
                {
                    TraceHelper.WriteLine("Not in lobby");
                }
                else
                {
                    TraceHelper.WriteLine("Peer name is not null and it's in a lobby");
                }
            }

            PlayersListBox.ItemsSource = items;
            IncomingChallengesListBox.ItemsSource = new List<ListBoxItem>();
            OutgoingChallengesListBox.ItemsSource = new List<ListBoxItem>();
        }

        private void subscribe(KulamiPeer peer)
        {
            peer.OnGameRequest += on_game_request;
            peer.OnGameResponse += on_game_response;
            peer.OnNameUpdate += on_name_update;
            peer.OnLobbyUpdate += on_lobby_update;
            peer.OnDisconnect += on_peer_disconnect;
        }

        private void unsubscribe(KulamiPeer peer)
        {
            peer.OnGameRequest -= on_game_request;
            peer.OnGameResponse -= on_game_response;
            peer.OnNameUpdate -= on_name_update;
            peer.OnLobbyUpdate -= on_lobby_update;
            peer.OnDisconnect -= on_peer_disconnect;
        }

        private void on_peer_discovery(object sender, EventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;

            subscribe(peer);

            if (string.IsNullOrEmpty(peer.Name) || !peer.InLobby) return;
            Dispatcher.Invoke(() =>
            {
                PlayersListBox.AddPeer(peer);
            });
        }

        private void on_peer_disconnect(object sender, EventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;

            Dispatcher.Invoke(() =>
            {
                PlayersListBox.RemovePeer(peer);
                IncomingChallengesListBox.RemovePeer(peer);
                OutgoingChallengesListBox.RemovePeer(peer);
            });
        }

        private void on_name_update(object sender, EventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;
            peer_update(peer);
        }

        private void on_lobby_update(object sender, LobbyEventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;
            peer_update(peer);
        }

        private void peer_update(KulamiPeer peer)
        {
            Dispatcher.Invoke(() =>
            {
                if (string.IsNullOrEmpty(peer.Name) || !peer.InLobby)
                {
                    PlayersListBox.RemovePeer(peer);
                    IncomingChallengesListBox.RemovePeer(peer);
                    OutgoingChallengesListBox.RemovePeer(peer);
                    return;
                }

                PlayersListBox.AddOrUpdatePeer(peer);
                IncomingChallengesListBox.UpdatePeer(peer);
                OutgoingChallengesListBox.UpdatePeer(peer);
            });
        }

        private void on_game_request(object sender, NetGameRequestEventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;

            Dispatcher.Invoke(() =>
            {
                show(IncomingChallengesCanvas);

                // Don't add them if they've already challenged us
                if(!IncomingChallengesListBox.Contains(peer))
                    IncomingChallengesListBox.AddPeer(peer);
            });
        }

        private void on_game_response(object sender, NetGameResponseEventArgs e)
        {
            KulamiPeer peer = sender as KulamiPeer;
            if (peer == null) return;

            if(e.ChallengeAccpeted)
                StartGame(peer, BoardSetup.GetBoard(e.BoardNum), e.ChallengerGoesFirst);
            else
            {
                Dispatcher.Invoke(() =>
                {
                    IncomingChallengesListBox.RemovePeer(peer);
                    if(!IncomingChallengesListBox.HasItems) hide(IncomingChallengesCanvas);
                });
            }
        }

        private static void StartGame(KulamiPeer peer, Board board, bool goFirst)
        {
            Networkmanager.InLobby = false;

            // Set up game engine
            HumanPlayer humanPlayer = new HumanPlayer(Networkmanager.ClientName, GameBoardUserControl.human_move_handler);
            NetworkPlayer networkPlayer = new NetworkPlayer(peer);

            GameBoardUserControl control = new GameBoardUserControl(
                board,
                humanPlayer,
                networkPlayer,
                goFirst ? PieceType.Red : PieceType.Black
                );

            ContentControlActions.setBaseContentControl(control);
        }

        private void Back_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Networkmanager.InLobby = false;

            Networkmanager.OnDiscovery -= on_peer_discovery;

            PeerHolder.Peers.ToList().ForEach(unsubscribe);

            ContentControlActions.FadeOut();
        }

        private void Challenge_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            KulamiPeer peer = PlayersListBox.SelectedPeer();
            if (peer == null) return;

            //todo: make this an animation instead
            if (OutgoingChallengesCanvas.Visibility == Visibility.Hidden)
            {
                OutgoingChallengesCanvas.Visibility = Visibility.Visible;
            }

            if (OutgoingChallengesListBox.Contains(peer))
            {
                OutgoingChallengesListBox.UpdatePeer(peer);
            }
            else
            {
                OutgoingChallengesListBox.AddPeer(peer);
            }

            // todo make this random number
            peer.SendRequest(5, true);
        }

        private void Connect_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            KulamiPeer peer = IncomingChallengesListBox.SelectedPeer();
            if (peer == null) return;

            peer.SendResponse(true);
            StartGame(peer,
                BoardSetup.GetBoard(peer.IncomingRequest.BoardNum),
                !peer.IncomingRequest.ChallengerGoesFirst);
        }

        private void ChallengeCancel_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            KulamiPeer peer = OutgoingChallengesListBox.SelectedPeer();
            if (peer == null) return;

            //todo: rescind challenge request for selected peer
            peer.SendResponse(false);

            OutgoingChallengesListBox.RemovePeer(peer);

            //todo: make this an animation instead
            if (!OutgoingChallengesListBox.HasItems)
            {
                OutgoingChallengesCanvas.Visibility = Visibility.Hidden;
            }
        }

        private void show(Canvas canvas)
        {
            // todo convert this and hide to animations
            if (canvas.Visibility == Visibility.Hidden)
            {
                canvas.Visibility = Visibility.Visible;
            }
        }

        private void hide(Canvas canvas)
        {
            if (canvas.Visibility == Visibility.Visible)
            {
                canvas.Visibility = Visibility.Hidden;
            }
        }
    }

    public static class ListBoxHelper
    {
        public static bool Contains(this ListBox listBox, KulamiPeer peer)
        {
            List<ListBoxItem> items = (List<ListBoxItem>) listBox.ItemsSource;
            return items
                .Select(x => (KulamiPeer) x.Content)
                .Any(x => x.Identifier == peer.Identifier);
        }

        public static void AddOrUpdatePeer(this ListBox listBox, KulamiPeer peer)
        {
            // Add the peer if we don't have it, or just update
            if (listBox.Contains(peer))
            {
                listBox.UpdatePeer(peer);
            }
            else
            {
                listBox.AddPeer(peer);
            }
        }

        public static void AddPeer(this ListBox listBox, KulamiPeer peer)
        {
            List<ListBoxItem> items = (List<ListBoxItem>)listBox.ItemsSource;
            items.Add(new ListBoxItem { Content = peer });
            listBox.ItemsSource = items;
            listBox.Items.Refresh();
        }

        public static void RemovePeer(this ListBox listBox, KulamiPeer peer)
        {
            List<ListBoxItem> items = (List<ListBoxItem>)listBox.ItemsSource;
            listBox.ItemsSource = items.Where(
                x => ((KulamiPeer)x.Content).Identifier != peer.Identifier
                ).ToList();
            listBox.Items.Refresh();
        }

        public static void UpdatePeer(this ListBox listBox, KulamiPeer peer)
        {
            if (!listBox.Contains(peer)) return;

            List<ListBoxItem> items = (List<ListBoxItem>) listBox.ItemsSource;

            int index = items.IndexOf(
                    items.SingleOrDefault(x => ((ListBoxItem) x).Content == peer)
                );

            items.RemoveAt(index);
            items.Insert(index, new ListBoxItem() { Content = peer});

            listBox.ItemsSource = items;
            listBox.Items.Refresh();
        }

        public static KulamiPeer SelectedPeer(this ListBox listBox)
        {
            ListBoxItem boxItem = listBox.SelectedItem as ListBoxItem;
            if (boxItem == null && listBox.Items.Count == 1)
            {
                boxItem = listBox.Items[0] as ListBoxItem;
            }

            if (boxItem == null) return null;

            KulamiPeer peer = boxItem.Content as KulamiPeer;

            return peer;
        }
    }
}
