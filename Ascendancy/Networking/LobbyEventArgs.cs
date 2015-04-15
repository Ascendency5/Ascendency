using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace networkTest
{
    public class LobbyEventArgs
    {
        public readonly bool InLobby;

        public LobbyEventArgs(bool inLobby)
        {
            InLobby = inLobby;
        }
    }
}
