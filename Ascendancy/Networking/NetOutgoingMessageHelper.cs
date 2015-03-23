using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ascendancy.Game_Engine;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public static class NetOutgoingMessageHelper
    {
        public static void Write(this NetOutgoingMessage message, Networkmanager.MessageType type)
        {
            message.Write((int)type);
        }

        public static void Write(this NetOutgoingMessage message, Packet packet)
        {
            message.Write(packet.Type);
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
                    Console.WriteLine("Writing a move");
                    Move move = (Move) data;
                    message.Write(move.Row);
                    message.Write(move.Col);
                }
            }
        }

        public static void Write(this NetOutgoingMessage message, NetPacketBuilder builder)
        {
            message.Write(builder.Count);
            foreach (Packet packet in builder)
            {
                message.Write(packet);
            }
        }
    }
}
