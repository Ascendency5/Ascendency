using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ascendancy.Game_Engine;
using Ascendancy.User_Controls.Multiplayer;

namespace Ascendancy.Networking
{
    public class NetworkPlayer : Player
    {
        private Move move;
        private Move lastMove;
        public readonly KulamiPeer Peer;
        private readonly string Name;

        public NetworkPlayer(KulamiPeer peer)
        {
            this.Peer = peer;
            move = Move.None;
            lastMove = Move.None;
            peer.OnPlayerMove += on_move_received;
            peer.OnChatMessage += on_chat_received;
            Name = peer.Name;
        }

        private void on_move_received(object sender, NetPlayerMoveEventArgs e)
        {
            // Accept the move if we don't have one and it's not the previous one
            if (move != Move.None || lastMove == e.Move) return;

            move = e.Move;
            lastMove = e.Move;
        }

        private void on_chat_received(object sender, NetChatEventArgs e)
        {
            
        }

        public Move GetMove(Board board, BoardState state)
        {
            while (move == Move.None)
                Thread.Yield();

            Move returnMove = move;
            move = Move.None;

            return returnMove;
        }

        public string GetName()
        {
            return Name;
        }

        public void Reset()
        {
            // todo Figure out how to reset the peer
        }

        public void on_player_move(object gameengine, PlayerMoveEventArgs eventargs)
        {
            if (eventargs.Player != this)
                sendThread(eventargs.Move);
        }

        private void sendThread(Move moveToSend)
        {
            Thread thread = new Thread(() =>
            {
                Peer.SendMove(moveToSend);
                Thread.Sleep(1000);
            }) {IsBackground = true};
            thread.Start();
        }
    }
}
