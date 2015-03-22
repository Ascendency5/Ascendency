using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Networking
{
    public class NetChatEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public NetChatEventArgs(string message)
        {
            Message = message;
        }
    }
}
