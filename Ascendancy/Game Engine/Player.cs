using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Game_Engine
{
    public interface Player
    {
        Move GetMove(Board board, BoardState state);
        string GetName();
        void Reset();
    }
}
