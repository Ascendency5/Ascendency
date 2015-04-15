using System;
using Ascendancy.Game_Engine;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public static class NetOutgoingMessageHelper
    {
        public static void Write(this NetOutgoingMessage message, NetMessageType type)
        {
            message.Write((int)type);
        }

        public static void Write(this NetOutgoingMessage message, Guid guid)
        {
            message.Write(guid.ToString());
        }

        public static void Write(this NetOutgoingMessage message, Packet packet)
        {
            message.Write(packet.Type);
            message.Write(packet.Token);
            foreach (var data in packet.Data)
            {
                if (data is string)
                {
                    message.Write((string)data);
                }
                else if (data is int)
                {
                    message.Write((int)data);
                }
                else if (data is bool)
                {
                    message.Write((bool)data);
                }
                else if (data is Move)
                {
                    Move move = (Move) data;
                    message.Write(move.Row);
                    message.Write(move.Col);
                }
            }
        }
    }
}
