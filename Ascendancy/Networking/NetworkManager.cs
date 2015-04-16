using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public static class Networkmanager
    {
        public static event EventHandler OnDiscovery;

        private const int Port = 47021;

        private static NetPeer Client { get; set; }

        private static string _ClientName;
        private static readonly object clientLock = new object();
        public static string ClientName
        {
            get
            {
                lock (clientLock)
                {
                    return _ClientName;
                }
            }
            set
            {
                lock (clientLock)
                {
                    _ClientName = value;
                    foreach (KulamiPeer peer in Peers.Values)
                    {
                        peer.SendName();
                    }
                }
            }
        }

        private static bool _InLobby;
        private static readonly object lobbyLock = new object();
        public static bool InLobby
        {
            get
            {
                lock (lobbyLock)
                {
                    return _InLobby;
                }
            }
            set
            {
                lock (lobbyLock)
                {
                    _InLobby = value;
                    TraceHelper.WriteLine("In lobby update: {0}", Peers.Count);
                    foreach (KulamiPeer peer in Peers.Values)
                    {
                        peer.SendLobby();
                    }
                }
            }
        }

        private static ConcurrentDictionary<long, KulamiPeer> Peers;

        public static void Start()
        {
            Peers = new ConcurrentDictionary<long, KulamiPeer>();

            NetPeerConfiguration netPeerConfiguration = new NetPeerConfiguration("HelloWorld Kulami")
            {
                AcceptIncomingConnections = true,
                AutoFlushSendQueue = true,
                PingInterval = 1f,
                //ResendHandshakeInterval = 1f,
                ConnectionTimeout = 7,
                Port = Port,
                UseMessageRecycling = false,
                MaximumHandshakeAttempts = 100
            };

            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            netPeerConfiguration.EnableMessageType(NetIncomingMessageType.StatusChanged);

            Client = new NetPeer(netPeerConfiguration);
            Client.RegisterReceivedCallback(on_received_mesage);
            Client.Start();

            TraceHelper.WriteLine("Personal ID: {0}", Client.UniqueIdentifier);
            TraceHelper.WriteLine("Broadcast: {0}", netPeerConfiguration.BroadcastAddress);

            // Broadcast thread
            new Thread(() =>
            {
                while (true)
                {
                    Client.DiscoverLocalPeers(Port);
                    Thread.Sleep(2000);
                }
            })
            {
                IsBackground = true
            }.Start();

            // Message queue
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);

                    // Loop through the clients, and tell them to send what's in their queue
                    foreach (KulamiPeer peer in Peers.Values)
                    {
                        peer.FlushQueue();
                    }
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        public static void Shutdown()
        {
            Client.Shutdown("Goodbye everyone!");
        }

        private static void on_received_mesage(object sender)
        {
            NetIncomingMessage incomingMessage = Client.ReadMessage();

            switch (incomingMessage.MessageType)
            {
                case NetIncomingMessageType.DiscoveryRequest:
                    //TraceHelper.WriteLine("Request from {0}", incomingMessage.SenderEndPoint);

                    //This message is received by the sender of the DiscoveryRequest
                    //as of type DiscoveryResponse
                    Client.SendDiscoveryResponse(null, incomingMessage.SenderEndPoint);
                    break;
                case NetIncomingMessageType.DiscoveryResponse:
                    //TraceHelper.WriteLine("Response from {0}", incomingMessage.SenderEndPoint);

                    // See if we have a connection from the SenderEndPoint
                    // If we don't, try to connect.

                    // todo fix already connected error
                    var connection = Client.GetConnection(incomingMessage.SenderEndPoint);
                    if (connection == null)
                    {
                        Client.Connect(incomingMessage.SenderEndPoint);
                    }
                    break;
                case NetIncomingMessageType.Data:
                    HandleData(incomingMessage);
                    break;
                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus) incomingMessage.ReadByte();
                    if (status == NetConnectionStatus.Disconnected)
                    {
                        if (Peers.ContainsKey(incomingMessage.SenderConnection.RemoteUniqueIdentifier))
                        {
                            KulamiPeer peer = Peers[incomingMessage.SenderConnection.RemoteUniqueIdentifier];
                            if (peer != null)
                            {
                                peer.Disconnect();
                            }
                        }
                    }
                    if (status == NetConnectionStatus.Connected)
                    {
                        AddConnection(incomingMessage.SenderConnection);
                    }
                    break;
            }
        }

        private static void AddConnection(NetConnection connection)
        {
            TraceHelper.WriteLine("RUI: {0}", connection.RemoteUniqueIdentifier);
            KulamiPeer peer;
            if (!Peers.ContainsKey(connection.RemoteUniqueIdentifier))
            {
                peer = new KulamiPeer(connection);
                Peers[connection.RemoteUniqueIdentifier] = peer;

                if(OnDiscovery != null)
                    OnDiscovery(peer,new EventArgs());
            }
            else
                Peers[connection.RemoteUniqueIdentifier].Connection = connection;

            peer = Peers[connection.RemoteUniqueIdentifier];
            peer.SendName();
            peer.SendLobby();
        }

        private static void HandleData(NetIncomingMessage message)
        {
            Packet packet = message.ReadPacket();

            Peers[message.SenderConnection.RemoteUniqueIdentifier].Receive(packet);
        }

        public static NetSendResult Send(KulamiPeer peer, Packet packet)
        {
            NetOutgoingMessage message = Client.CreateMessage();
            message.Write(packet);

            return Client.SendMessage(message, peer.Connection, NetDeliveryMethod.ReliableOrdered, 0);
        }
    }
}
