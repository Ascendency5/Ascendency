using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ascendancy.Game_Engine;

namespace Ascendancy.Networking
{
    public static class NetPacketHandler
    {

        public static void HandleName(KulamiPeer peer, Packet namePacket)
        {
            if (namePacket == null)
            {
                peer.Name = null;
                return;
            }

            peer.Name = (string) namePacket.Data[0];
        }

        public static void HandleChat(KulamiPeer peer, Packet[] chatPackets)
        {
            foreach (Packet chatPacket in chatPackets.Where(
                chatPacket => chatPacket != null && peer.OnChatMessage != null
                ))
            {
                peer.OnChatMessage(peer, new NetChatEventArgs((string)chatPacket.Data[0]));
            }
        }

        public static void HandleGameRequest(KulamiPeer peer, Packet gameRequestPacket)
        {
            if (gameRequestPacket == null) return;

            // Data[0] is board #
            // Data[1] is if the challenger goes first
            NetGameRequestEventArgs eventArgs = new NetGameRequestEventArgs(
                (int)gameRequestPacket.Data[0],
                (bool)gameRequestPacket.Data[1]
                );

            peer.IncomingRequest = eventArgs;

            if (peer.OnGameRequest != null)
            {
                peer.OnGameRequest(peer, eventArgs);
            }
        }

        public static void HandleGameResponse(KulamiPeer peer, Packet gameResponsePacket)
        {
            if (gameResponsePacket == null || peer.OnGameResponse == null || peer.OutgoingRequest == null) return;

            bool gameAccepted = (bool)gameResponsePacket.Data[0];
            peer.OnGameResponse(peer, new NetGameResponseEventArgs(
                peer.OutgoingRequest.BoardNum,
                peer.OutgoingRequest.ChallengerGoesFirst,
                gameAccepted
                ));
        }

        public static void HandleMove(KulamiPeer peer, Packet movePacket)
        {
            if (movePacket == null || peer.OnPlayerMove == null) return;

            Move move = new Move((int)movePacket.Data[0], (int)movePacket.Data[1]);
            peer.OnPlayerMove(peer, new NetPlayerMoveEventArgs(move));
        }
    }
}
