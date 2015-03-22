using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Networking
{
    public static class PeerHolder
    {
        public static ConcurrentSet<KulamiPeer> Peers = new ConcurrentSet<KulamiPeer>(); 

        public static void on_peer_discovery(object sender, EventArgs e)
        {
            Peers.Add((KulamiPeer) sender);
        }

        public static void on_peer_disconnect(object sender, EventArgs e)
        {
            KulamiPeer peer = (KulamiPeer) sender;
            Peers.Remove(peer);
        }
    }
}
