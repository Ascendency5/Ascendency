using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Networking
{
    public class NetGameResponseEventArgs : NetGameRequestEventArgs
    {
        public bool ChallengeAccpeted { get; private set; }

        public NetGameResponseEventArgs(int boardNum, bool challengerGoesFirst, bool challengeAccepted) :
            base(boardNum, challengerGoesFirst)
        {
            ChallengeAccpeted = challengeAccepted;
        }
    }
}
