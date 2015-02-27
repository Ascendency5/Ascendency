using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Game_Engine
{
    public class GameBoardEventArgs : EventArgs
    {
        public readonly Board Board;
        public readonly BoardState State;

        public GameBoardEventArgs(Board board, BoardState state)
        {
            this.Board = board;
            this.State = state;
        }
    }
}
