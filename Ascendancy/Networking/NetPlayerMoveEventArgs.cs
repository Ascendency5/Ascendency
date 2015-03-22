using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ascendancy.Game_Engine;

namespace Ascendancy.Networking
{
    public class NetPlayerMoveEventArgs : EventArgs
    {
        public Move Move { get; set; }
        public NetPlayerMoveEventArgs(Move move)
        {
            Move = move;
        }
    }
}
