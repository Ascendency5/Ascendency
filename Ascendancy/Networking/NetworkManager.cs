using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Ascendancy.Game_Engine;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public static class Networkmanager
    {
        public static event EventHandler OnDiscovery;
        public static event EventHandler OnDisconnect;

        public enum MessageType
        {
            Ack,
            Chat,
            Name,
            Identifier,
            GameRequest,
            GameResponse,
            PlayerMove
        }

        public const int Port = 47021;

        public static string Identifier { get; private set; }
        private static NetPeer Client { get; set; }
        public static string ClientName { get; set; }

        private static CancellationTokenSource cancelSource;
        private static BroadcastThread broadcastThread;

        public static void Start()
        {
            Identifier = Guid.NewGuid().ToString();

            NetPeerConfiguration netPeerConfiguration = new NetPeerConfiguration("HelloWorld Kulami")
            {
                AcceptIncomingConnections = true,
                AutoFlushSendQueue = true,
                ConnectionTimeout = 7,
                Port = Port
            };

            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.UnconnectedData);

            Client = new NetPeer(netPeerConfiguration);
            Client.RegisterReceivedCallback(on_received_mesage);
            Client.Start();

            cancelSource = new CancellationTokenSource();
            broadcastThread = new BroadcastThread(Client, cancelSource.Token);
            broadcastThread.Start();
        }

        public static void Shutdown()
        {
            cancelSource.Cancel();
            Client.Shutdown("Goodbye everyone!");
        }

        private static void on_received_mesage(object sender)
        {
            NetIncomingMessage incomingMessage = Client.ReadMessage();
            NetOutgoingMessage responseMessage;
            NetPacketBuilder builder;

            switch (incomingMessage.MessageType)
            {
                case NetIncomingMessageType.DiscoveryRequest:
                    //TraceHelper.WriteLine("Request from {0}", incomingMessage.SenderEndPoint);
                    builder = new NetPacketBuilder();
                    builder.Add(new Packet(MessageType.Ack, "Request"));

                    responseMessage = Client.CreateMessage();
                    responseMessage.Write(builder);

                    //This message is received by the sender of the DiscoveryRequest
                    //as of type DiscoveryResponse
                    Client.SendDiscoveryResponse(responseMessage, incomingMessage.SenderEndPoint);

                    break;

                case NetIncomingMessageType.DiscoveryResponse:
                    //TraceHelper.WriteLine("Response from {0}", incomingMessage.SenderEndPoint);
                    KulamiPeer peer = HandleData(incomingMessage);

                    builder = new NetPacketBuilder();
                    builder.Add(new Packet(MessageType.Ack, "Response"));

                    responseMessage = Client.CreateMessage();
                    responseMessage.Write(builder);

                    //Sends a ConnectionApproval message to the sender
                    //of the DiscoveryResponse message.
                    if (peer != null && peer.Connection == null)
                    {
                        NetConnection connection = Client.Connect(incomingMessage.SenderEndPoint.Address.ToString(),
                            incomingMessage.SenderEndPoint.Port, responseMessage);
                        peer.Connection = connection;
                    }
                    break;

                case NetIncomingMessageType.ConnectionApproval: 
                    //TraceHelper.WriteLine("Connection from {0}", incomingMessage.SenderEndPoint);
                    //Establishes the connection between ourselves
                    //and the sender of the ConnectionApproval
                    //message
                    incomingMessage.SenderConnection.Approve();
                    HandleData(incomingMessage);

                    break;

                case NetIncomingMessageType.Data:
                    HandleData(incomingMessage);
                     
                    break;
            }
        }

        public static void Send(KulamiPeer peer, Packet packet)
        {
            if(peer.Identifier != Identifier)
                broadcastThread.AddPacket(peer, packet);
        }

        private static KulamiPeer HandleData(NetIncomingMessage message)
        {
            Packet[] packets = Packet.Read(message);

            KulamiPeer peer;

            packets = broadcastThread.Filter(out peer, packets);

            if (peer == null) return null;

            Trace.WriteLine("Receiving data");
            foreach (Packet packet in packets)
            {
                Trace.WriteLine(packet);
            }

            KulamiPeer existingPeer = PeerHolder.Peers.SingleOrDefault(x => x.Identifier == peer.Identifier);
            bool newPeer = true;
            if (existingPeer != null)
            {
                peer = existingPeer;
                newPeer = false;
            }

            string name = peer.Name;
            NetPacketHandler.HandleName(peer, packets.Type(MessageType.Name));

            var updatedPeer = peer.Name != name;

            // The timstamp doesn't mean that the peer has updated information
            peer.UpdateTimestamp();

            if (peer.Connection == null && message.SenderConnection != null)
            {
                peer.Connection = message.SenderConnection;
                updatedPeer = true;
            }

            if (newPeer)
            {
                if (OnDiscovery != null)
                {
                    OnDiscovery(peer, new EventArgs());
                }
            }
            else if (updatedPeer)
            {
                if (peer.OnUpdate != null)
                {
                    peer.OnUpdate(peer, new EventArgs());
                }
            }

            NetPacketHandler.HandleChat(peer, packets.AllType(MessageType.Chat));
            NetPacketHandler.HandleGameRequest(peer, packets.Type(MessageType.GameRequest));
            NetPacketHandler.HandleGameResponse(peer, packets.Type(MessageType.GameResponse));
            NetPacketHandler.HandleMove(peer, packets.Type(MessageType.PlayerMove));

            return peer;
        }

        public static void disconnect(List<KulamiPeer> peers)
        {
            if (OnDisconnect == null) return;

            foreach (KulamiPeer peer in peers)
            {
                OnDisconnect(peer, new EventArgs());
            }
        }
    }

    public static class PacketHelper
    {
        public static Packet Type(this Packet[] packets, Networkmanager.MessageType type)
        {
            return packets.SingleOrDefault(x => x.Type == type);
        }

        public static Packet[] AllType(this Packet[] packets, Networkmanager.MessageType type)
        {
            return packets.Where(x => x.Type == type).ToArray();
        }
    }
}
