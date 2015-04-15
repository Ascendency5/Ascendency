using System;

namespace Ascendancy.Networking
{
    public class NetNameEventArgs : EventArgs
    {
        public readonly string Name;

        public NetNameEventArgs(string name)
        {
            Name = name;
        }
    }
}
