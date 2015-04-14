using System;
using System.Collections.Generic;
using Ascendancy.Game_Engine;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public class KulamiPeer
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public NetConnection Connection { get; set; }

        public DateTime LastSeen { get; private set; }

        public EventHandler<NetChatEventArgs> OnChatMessage;
        public EventHandler<NetGameRequestEventArgs> OnGameRequest;
        public EventHandler<NetGameResponseEventArgs> OnGameResponse;
        public EventHandler<NetPlayerMoveEventArgs> OnPlayerMove;
        public EventHandler OnUpdate;

        public NetGameRequestEventArgs IncomingRequest { get; set; }
        public NetGameRequestEventArgs OutgoingRequest { get; private set; }

        public void SendChat(string message)
        {
            NetPacketBuilder builder = new NetPacketBuilder
            {
                new Packet(Networkmanager.MessageType.Chat, message)
            };

            Networkmanager.Send(Connection, builder);
        }

        public void SendRequest(int boardNum, bool challengerGoesFirst)
        {
            OutgoingRequest = new NetGameRequestEventArgs(
                       boardNum,
                       challengerGoesFirst
                       );
            NetPacketBuilder builder = new NetPacketBuilder()
            {
                new Packet(Networkmanager.MessageType.GameRequest, boardNum, challengerGoesFirst)
            };
            Networkmanager.Send(Connection, builder);
        }

        public void SendMove(Move move)
        {
            NetPacketBuilder builder = new NetPacketBuilder()
            {
                new Packet(Networkmanager.MessageType.PlayerMove, move)
            };

            Networkmanager.Send(Connection, builder);
        }

        public void UpdateTimestamp()
        {
            LastSeen = DateTime.Now;
        }

        public void SendResponse(bool challengeAccepted)
        {
            NetPacketBuilder builder = new NetPacketBuilder()
            {
                new Packet(Networkmanager.MessageType.GameResponse, challengeAccepted)
            };

            Networkmanager.Send(Connection, builder);
        }

        public override string ToString()
        {
            return Name ?? "null";
        }
    }
}
