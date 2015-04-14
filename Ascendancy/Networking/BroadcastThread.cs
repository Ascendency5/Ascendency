using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public class BroadcastThread
    {
        private CancellationToken cancelToken;
        private NetPeer client;

        private ConcurrentDictionary<string, ConcurrentSet<Packet>> packetsToSend = new ConcurrentDictionary<string, ConcurrentSet<Packet>>();  

        public BroadcastThread(NetPeer client, CancellationToken token)
        {
            cancelToken = token;
            this.client = client;
        }

        public void Start()
        {
            new Thread(run)
            {
                IsBackground = true
            }.Start();
        }

        private void run()
        {
            while (!cancelToken.IsCancellationRequested)
            {
                //Broadcast sent to other computers listening on this same port.
                //Other computers receive this message as of type DiscoveryRequest
                client.DiscoverLocalPeers(Networkmanager.Port);

                Thread.Sleep(100);

                List<KulamiPeer> disconnectablePeers =
                    PeerHolder.Peers.Where(x => x.LastSeen < DateTime.Now.AddSeconds(-200)).ToList();

                Networkmanager.disconnect(disconnectablePeers);

                List<KulamiPeer> connectedPeers = PeerHolder.Peers.Where(x => x.Connection != null).ToList();

                foreach (KulamiPeer peer in connectedPeers)
                {
                    if(Networkmanager.ClientName != null)
                        AddPacket(peer, new Packet(Networkmanager.MessageType.Name, Networkmanager.ClientName));

                    NetPacketBuilder builder = new NetPacketBuilder();
                    Packet[] packets = GetPackets(peer).ToArray();
                    if (Networkmanager.ClientName == null)
                        packets = packets.Where(x => x.Type != Networkmanager.MessageType.Name).ToArray();
                    foreach (Packet packet in packets)
                    {
                        builder.Add(packet);
                    }

                    Send(peer.Connection, builder);
                }
            }
        }

        private void Send(NetConnection connection, NetPacketBuilder builder)
        {
            NetOutgoingMessage message = client.CreateMessage();
            message.Write(builder);

            connection.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
        }

        public Packet[] Filter(out KulamiPeer peer, Packet[] packets)
        {
            Packet idPacket = packets.Type(Networkmanager.MessageType.Identifier);
            string identifier = (string) idPacket.Data[0];

            // That's our identifier, ignore it
            if (Networkmanager.Identifier == identifier)
            {
                peer = null;
                return new Packet[0];
            }

            peer = PeerHolder.Peers.SingleOrDefault(x => x.Identifier == identifier) ??
                new KulamiPeer()
                {
                    Identifier = identifier
                };

            List<string> ackTokens = GetPackets(peer)
                .Where(x => x.Type == Networkmanager.MessageType.Ack)
                .Select(x => x.Token)
                .ToList();

            // Ack everything except the Identifier, Name, and Ack packets
            // Those packet must always be sent
            foreach (Packet packet in packets.Where(
                x => x.Type != Networkmanager.MessageType.Identifier &&
                    x.Type != Networkmanager.MessageType.Name &&
                    x.Type != Networkmanager.MessageType.Ack
                ))
            {
                AddPacket(peer, new Packet(Networkmanager.MessageType.Ack, packet.Token));
            }

            // todo You're not acking correctly, as in not removing the packets that we get that are ack packets
            // todo We're also not filtering messages out because of that, so we get the game response over and over again

            // Remove the packets that have been acked.
            GetPackets(peer).RemoveAll(packets.Where(x => x.Type == Networkmanager.MessageType.Ack).ToArray());

            return packets.Where(packet => !ackTokens.Contains(packet.Token)).ToArray();
        }

        public void AddPacket(KulamiPeer peer, Packet packet)
        {
            ConcurrentSet<Packet> packets = GetPackets(peer);

            // Only chat and ack packets can have multiple packets to send
            if (packet.Type != Networkmanager.MessageType.Ack &&
                packet.Type != Networkmanager.MessageType.Chat)
            {
                Packet[] removePackets = packets.Where(x => x.Type == packet.Type).ToArray();
                packets.RemoveAll(removePackets);
            }

            if (!packets.Contains(packet))
                packets.Add(packet);
        }

        public ConcurrentSet<Packet> GetPackets(KulamiPeer peer)
        {
            if (!packetsToSend.ContainsKey(peer.Identifier))
            {
                packetsToSend[peer.Identifier] = new ConcurrentSet<Packet>();
            }

            return packetsToSend[peer.Identifier];
        }
    }
}
