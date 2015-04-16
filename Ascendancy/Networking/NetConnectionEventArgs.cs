using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Networking
{
    public class NetConnectionEventArgs : EventArgs
    {
        public readonly bool Connected;

        public NetConnectionEventArgs(bool connected)
        {
            Connected = connected;
        }
    }
}
