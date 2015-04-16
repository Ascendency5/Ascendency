using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Ascendancy.Game_Engine;
using Lidgren.Network;
using networkTest;
using Timer = System.Windows.Forms.Timer;

namespace Ascendancy.Networking
{
    public class KulamiPeer
    {
        public string Name { get; private set; }

        private bool _connected;
        public bool Connected
        {
            get
            {
                return _connected;
            }
            private set
            {
                _connected = value;
                if (OnConnectionChange == null) return;

                OnConnectionChange(this, new NetConnectionEventArgs(_connected));
            }
        }

        private NetConnection connection;
        private readonly object connectionLock = new object();
        public NetConnection Connection
        {
            get
            {
                lock (connectionLock)
                {
                    return connection;
                }
            }
            set
            {
                lock (connectionLock)
                {
                    Connected = true;
                    connection = value;
                    disconnectTimer.Stop();
                }
            }
        }

        public long Identifier
        {
            get { return Connection.RemoteUniqueIdentifier; }
        }

        public bool InLobby { get; private set; }

        public EventHandler<NetChatEventArgs> OnChatMessage;
        public EventHandler<NetGameRequestEventArgs> OnGameRequest;
        public EventHandler<NetGameResponseEventArgs> OnGameResponse;
        public EventHandler<NetPlayerMoveEventArgs> OnPlayerMove;
        public EventHandler<NetNameEventArgs> OnNameUpdate;
        public EventHandler<LobbyEventArgs> OnLobbyUpdate;
        public EventHandler<NetConnectionEventArgs> OnConnectionChange;
        public EventHandler OnPlayerLeave;

        public NetGameRequestEventArgs IncomingRequest { get; set; }
        public NetGameRequestEventArgs OutgoingRequest { get; private set; }

        private readonly ConcurrentDictionary<Packet, int> sendAttempts;
        private readonly ConcurrentSet<Guid> seenTokens;

        private readonly Timer disconnectTimer = new Timer();

        public KulamiPeer(NetConnection connection)
        {
            Connection = connection;
            sendAttempts = new ConcurrentDictionary<Packet, int>();
            seenTokens = new ConcurrentSet<Guid>();
            disconnectTimer.Tick += disconnect_callback;
            disconnectTimer.Interval = 1500;
        }

        private void disconnect_callback(object sender, EventArgs e)
        {
            Connected = false;
            disconnectTimer.Stop();
        }

        public void Disconnect()
        {
            disconnectTimer.Start();
        }

        public void FlushQueue()
        {
            // Try to send all packets
            foreach (Packet packet in sendAttempts.Keys)
            {
                NetSendResult result = Send(packet);

                // Exit if we can't send a packet
                if (result != NetSendResult.Sent)
                    break;
            }
        }

        private NetSendResult Send(Packet packet)
        {
            // Make sure we have a record if the packet
            if (!sendAttempts.ContainsKey(packet))
                sendAttempts[packet] = 0;

            NetSendResult result = Networkmanager.Send(this, packet);

            if (result == NetSendResult.Sent)
            {
                sendAttempts[packet]++;
            }

            // Send it 20 times
            if (sendAttempts[packet] <= 20) return result;

            int sentCount;
            sendAttempts.TryRemove(packet, out sentCount);
            return result;
        }

        public void SendLobby()
        {
            Send(Packet.Create(NetMessageType.InLobby, Networkmanager.InLobby));
        }

        public void SendName()
        {
            Send(Packet.Create(NetMessageType.Name, Networkmanager.ClientName));
        }

        public void SendChat(string message)
        {
            Send(Packet.Create(NetMessageType.Chat, message));
        }

        public void SendRequest(int boardNum, bool challengerGoesFirst)
        {
            OutgoingRequest = new NetGameRequestEventArgs(
                       boardNum,
                       challengerGoesFirst
                       );
            Send(Packet.Create(NetMessageType.GameRequest, boardNum, challengerGoesFirst));
        }

        public void SendMove(Move move)
        {
            Send(Packet.Create(NetMessageType.PlayerMove, move));
        }

        public void SendResponse(bool challengeAccepted)
        {
            Send(Packet.Create(NetMessageType.GameResponse, challengeAccepted));
        }

        public void SendLeaveGame()
        {
            Send(Packet.Create(NetMessageType.Leave));
        }

        private bool Seen(Packet packet)
        {
            return seenTokens.Contains(packet.Token);
        }

        public void Receive(Packet packet)
        {
            if (Seen(packet)) return;

            seenTokens.Add(packet.Token);

            string result = packet.Data.Aggregate("", (current, data) => current + (" " + data));
            TraceHelper.WriteLine("{2}: {0}: {1}", packet.Type, result, packet.Token);

            switch (packet.Type)
            {
                case NetMessageType.InLobby:
                    bool newLobby = (bool) packet.Data[0];
                    if (InLobby == newLobby)
                        break;
                    InLobby = newLobby;
                    if (OnLobbyUpdate != null)
                        OnLobbyUpdate(this, new LobbyEventArgs(InLobby));
                    break;
                case NetMessageType.Chat:
                    if(OnChatMessage != null)
                        OnChatMessage(this, new NetChatEventArgs((string) packet.Data[0]));
                    break;
                case NetMessageType.Name:
                    string newName = (string) packet.Data[0];
                    if (Name == newName)
                        break;
                    Name = newName;
                    if(OnNameUpdate != null)
                        OnNameUpdate(this, new NetNameEventArgs(Name));
                    break;
                case NetMessageType.GameRequest:
                    IncomingRequest = new NetGameRequestEventArgs(
                        (int) packet.Data[0],
                        (bool) packet.Data[1]
                        );
                    if(OnGameRequest != null)
                        OnGameRequest(this, IncomingRequest);
                    break;
                case NetMessageType.GameResponse:
                    if (OnGameResponse != null)
                    {
                        bool challengeAccepted = (bool) packet.Data[0];
                        NetGameResponseEventArgs response;
                        if (OutgoingRequest == null)
                            response = new NetGameResponseEventArgs(0, true, challengeAccepted);
                        else
                        {
                            response = new NetGameResponseEventArgs(
                                OutgoingRequest.BoardNum,
                                OutgoingRequest.ChallengerGoesFirst,
                                challengeAccepted
                                );
                        }
                        OnGameResponse(this, response);
                    }
                    break;
                case NetMessageType.PlayerMove:
                    if(OnPlayerMove != null)
                        OnPlayerMove(this, new NetPlayerMoveEventArgs(
                            (Move) packet.Data[0]
                            ));
                    break;
                case NetMessageType.Leave:
                    if (OnPlayerLeave != null)
                        OnPlayerLeave(this, new EventArgs());
                    break;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
