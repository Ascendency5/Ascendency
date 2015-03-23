using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Lidgren.Network;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ascendancy.Game_Engine;

namespace Ascendancy.Networking
{
    public static class Networkmanager
    {
        public static event EventHandler OnDiscovery;
        public static event EventHandler OnDisconnect;

        public enum MessageType
        {
            EMPTY,
            Chat,
            Name,
            Identifier,
            GameRequest,
            GameResponse,
            PlayerMove
        }

        const int PORT = 47020;

        public static string Identifier { get; private set; }
        public static NetPeer Client { get; private set; }
        public static string ClientName { get; set; }

        private static Thread broadcastThread;

        private static List<KulamiPeer> peers;

        public static void Start()
        {
            peers = new List<KulamiPeer>();

            Identifier = Guid.NewGuid().ToString();

            NetPeerConfiguration netPeerConfiguration = new NetPeerConfiguration("HelloWorld Kulami")
            {
                AcceptIncomingConnections = true,
                AutoFlushSendQueue = true,
                ConnectionTimeout = 10,
                Port = PORT
            };

            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.UnconnectedData);

            Client = new NetPeer(netPeerConfiguration);
            Client.RegisterReceivedCallback(on_received_mesage);
            Client.Start();

            broadcastThread = new Thread(repeat_broadcast) { IsBackground = true };
            broadcastThread.Start();
        }

        public static void Shutdown()
        {
            broadcastThread.Abort();
            Client.Shutdown("Goodbye everyone!");
        }

        private static void repeat_broadcast()
        {
            while (true)
            {
                //Broadcast sent to other computers listening on this same port.
                //Other computers receive this message as of type DiscoveryRequest
                Client.DiscoverLocalPeers(PORT);
                // Sleep for 2 seconds
                Thread.Sleep(2000);

                // todo Add thread safe operation
                List<KulamiPeer> disconnectedPeers = peers.Where(x => x.LastSeen < DateTime.Now.AddSeconds(-10))
                    .ToList();

                if (OnDisconnect != null)
                {
                    foreach (KulamiPeer peer in disconnectedPeers)
                    {
                        OnDisconnect(peer, new EventArgs());
                    }
                }

                peers.RemoveAll(x => disconnectedPeers.Contains(x));

                List<KulamiPeer> connectedPeers = peers.Where(x => x.Connection != null).ToList();
                NetPacketBuilder builder = new NetPacketBuilder();
                foreach (KulamiPeer peer in connectedPeers)
                {
                    Send(peer.Connection, builder);
                }
            }
        }

        private static void on_received_mesage(object _peer)
        {
            NetIncomingMessage incomingMessage = Client.ReadMessage();
            NetOutgoingMessage responseMessage;
            NetPacketBuilder builder;

            switch (incomingMessage.MessageType)
            {
                case NetIncomingMessageType.DiscoveryRequest:
                    //Console.WriteLine("Request from {0}", incomingMessage.SenderEndPoint);
                    builder = new NetPacketBuilder();

                    responseMessage = Client.CreateMessage();
                    responseMessage.Write(builder);

                    //This message is received by the sender of the DiscoveryRequest
                    //as of type DiscoveryResponse
                    Client.SendDiscoveryResponse(responseMessage, incomingMessage.SenderEndPoint);

                    break;

                case NetIncomingMessageType.DiscoveryResponse:
                    //Console.WriteLine("Response from {0}", incomingMessage.SenderEndPoint);
                    KulamiPeer peer = handleData(incomingMessage);

                    builder = new NetPacketBuilder();

                    responseMessage = Client.CreateMessage();
                    responseMessage.Write(builder);

                    //Sends a ConnectionApproval message to the sender
                    //of the DiscoveryResponse message.
                    NetConnection connection = Client.Connect(incomingMessage.SenderEndPoint.Address.ToString(), incomingMessage.SenderEndPoint.Port, responseMessage);
                    if (peer != null)
                        peer.Connection = connection;
                    break;

                case NetIncomingMessageType.ConnectionApproval: 
                    //Console.WriteLine("Connection from {0}", incomingMessage.SenderEndPoint);
                    //Establishes the connection between ourselves
                    //and the sender of the ConnectionApproval
                    //message
                    incomingMessage.SenderConnection.Approve();
                    handleData(incomingMessage);

                    break;

                case NetIncomingMessageType.Data:
                    handleData(incomingMessage);
                     
                    break;
            }
        }

        private static KulamiPeer handleData(NetIncomingMessage message)
        {
            // todo: Keep up with packets:
            // (DONE)1. Don't respond if there is no request
            // (DONE)2. Remove the request when a response is received
            // 3. Cancel existing requests if accepted
            // 4. Deny all other challenges if request accepted
            Packet[] packets = Packet.Read(message);

            Packet idPacket = packets.Type(MessageType.Identifier);

            string identifier = (string)idPacket.Data[0];

            if (identifier == Identifier)
                return null;

            /**
            foreach (Packet packet in packets)
            {
                Console.WriteLine(packet);
            }
             */

            KulamiPeer peer = peers.SingleOrDefault(x => x.Identifier == identifier);
            bool newPeer = false;
            if (peer == null)
            {
                peer = new KulamiPeer {Identifier = identifier};
                newPeer = true;
            }

            Packet namePacket = packets.Type(MessageType.Name);
            if (namePacket != null)
            {
                peer.Name = (string)namePacket.Data[0];
            }

            if (peer.Connection == null && message.SenderConnection != null)
                peer.Connection = message.SenderConnection;

            peer.UpdateTimestamp();

            if (newPeer)
            {
                peers.Add(peer);
                if (OnDiscovery != null)
                {
                    // On Discovery handler
                    OnDiscovery(peer, new EventArgs());
                }
            }

            Packet chatPacket = packets.Type(MessageType.Chat);
            if (chatPacket != null && peer.OnChatMessage != null)
            {
                peer.OnChatMessage(peer, new NetChatEventArgs((string)chatPacket.Data[0]));
            }

            Packet gameRequestPacket = packets.Type(MessageType.GameRequest);
            if (gameRequestPacket != null && peer.OnGameRequest != null)
            {
                 // Data[0] is board #
                 // Data[1] is if the challenger goes first
                NetGameRequestEventArgs eventArgs = new NetGameRequestEventArgs(
                    (int) gameRequestPacket.Data[0],
                    (bool) gameRequestPacket.Data[1]
                    );
                peer.OnGameRequest(peer, eventArgs);
            }

            Packet gameResponsePacket = packets.Type(MessageType.GameResponse);
            if (gameResponsePacket != null && peer.OnGameResponse != null && peer.Request != null)
            {
                peer.OnGameResponse(peer, new NetGameResponseEventArgs(
                    peer.Request.BoardNum,
                    peer.Request.ChallengerGoesFirst,
                    (bool)gameResponsePacket.Data[0]
                    ));
            }

            Packet movePacket = packets.Type(MessageType.PlayerMove);
            if (movePacket != null && peer.OnPlayerMove != null)
            {
                Move move = new Move((int)movePacket.Data[0], (int) movePacket.Data[1]);
                peer.OnPlayerMove(peer, new NetPlayerMoveEventArgs(move));
            }

            return peer;
        }

        public static void Send(NetConnection connection, NetPacketBuilder builder)
        {
            NetOutgoingMessage message = Client.CreateMessage();
            message.Write(builder);

            connection.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }

    public static class PacketHelper
    {
        public static Packet Type(this Packet[] packets, Networkmanager.MessageType type)
        {
            return packets.SingleOrDefault(x => x.Type == type);
        }
    }
}
