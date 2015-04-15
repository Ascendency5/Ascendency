using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Networking
{
    public enum NetMessageType
    {
        InLobby,
        Chat,
        Name,
        GameRequest,
        GameResponse,
        PlayerMove
    }
}
