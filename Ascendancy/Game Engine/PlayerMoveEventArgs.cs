using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Game_Engine
{
    public class PlayerMoveEventArgs : PlayerEventArgs
    {
        public readonly Move Move;

        public PlayerMoveEventArgs(Board board, BoardState state, Player player, Move move)
            : base(board, state, player)
        {
            this.Move = move;
        }
    }
}
