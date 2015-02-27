using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Game_Engine
{
    public class PlayerEventArgs : GameBoardEventArgs
    {
        public readonly Player Player;

        public PlayerEventArgs(Board board, BoardState state, Player player)
            : base(board, state)
        {
            this.Player = player;
        }
    }
}
