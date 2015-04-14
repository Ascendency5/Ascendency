using System;
using System.Collections.Generic;

namespace Ascendancy.Networking
{
    public class NetPacketBuilder : IEnumerable<Packet>
    {
        private List<Packet> packets;

        public NetPacketBuilder()
        {
            packets = new List<Packet> {new Packet(Networkmanager.MessageType.Identifier, Networkmanager.Identifier)};
            //if(Networkmanager.ClientName != null)
            //    packets.Add(new Packet(Networkmanager.MessageType.Name, Networkmanager.ClientName));
        }

        public int Count { get { return packets.Count; } }

        public IEnumerator<Packet> GetEnumerator()
        {
            return packets.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public NetPacketBuilder Add(Packet packet)
        {
            packets.Add(packet);
            return this;
        }
    }
}