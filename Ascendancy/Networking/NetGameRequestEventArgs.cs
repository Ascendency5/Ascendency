using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Networking
{
    public class NetGameRequestEventArgs : EventArgs
    {
        public int BoardNum { get; private set; }
        public bool ChallengerGoesFirst { get; private set; }

        public NetGameRequestEventArgs(int boardNum, bool challengerGoesFirst)
        {
            BoardNum = boardNum;
            ChallengerGoesFirst = challengerGoesFirst;
        }
    }
}
