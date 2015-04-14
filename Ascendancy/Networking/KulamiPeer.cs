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
            Networkmanager.Send(this, new Packet(Networkmanager.MessageType.Chat, message));
        }

        public void SendRequest(int boardNum, bool challengerGoesFirst)
        {
            OutgoingRequest = new NetGameRequestEventArgs(
                       boardNum,
                       challengerGoesFirst
                       );
            Networkmanager.Send(this, new Packet(Networkmanager.MessageType.GameRequest, boardNum, challengerGoesFirst));
        }

        public void SendMove(Move move)
        {
            Networkmanager.Send(this, new Packet(Networkmanager.MessageType.PlayerMove, move));
        }

        public void UpdateTimestamp()
        {
            LastSeen = DateTime.Now;
        }

        public void SendResponse(bool challengeAccepted)
        {
            Networkmanager.Send(this, new Packet(Networkmanager.MessageType.GameResponse, challengeAccepted));
        }

        public override string ToString()
        {
            return Name ?? "null";
        }
    }
}
