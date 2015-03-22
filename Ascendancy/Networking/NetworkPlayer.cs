using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ascendancy.Game_Engine;

namespace Ascendancy.Networking
{
    public class NetworkPlayer : Player
    {
        private Move move;
        private KulamiPeer peer;
        public NetworkPlayer(KulamiPeer peer)
        {
            this.peer = peer;
            move = Move.None;
            peer.OnPlayerMove += on_move_received;
        }

        private void on_move_received(object sender, NetPlayerMoveEventArgs e)
        {
            move = e.Move;
        }

        public Move getMove(Board board, BoardState state)
        {
            while (move == Move.None)
                Thread.Yield();

            Move returnMove = move;
            move = Move.None;

            return returnMove;
        }

        public void on_player_move(object gameengine, PlayerMoveEventArgs eventargs)
        {
            if(eventargs.Player != this)
                peer.SendMove(eventargs.Move);
        }
    }
}
