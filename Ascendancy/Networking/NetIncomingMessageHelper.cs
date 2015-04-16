using System;
using System.Collections.Generic;
using Ascendancy.Game_Engine;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public static class NetIncomingMessageHelper
    {
        public static Guid ReadGuid(this NetIncomingMessage message)
        {
            return Guid.Parse(message.ReadString());
        }

        public static Packet ReadPacket(this NetIncomingMessage message)
        {
            NetMessageType type = (NetMessageType)message.ReadInt32();
            Guid token = message.ReadGuid();
            List<object> data = new List<object>();

            switch (type)
            {
                case NetMessageType.InLobby:
                    data.Add(message.ReadBoolean());
                    break;
                case NetMessageType.Chat:
                    data.Add(message.ReadString());
                    break;
                case NetMessageType.GameRequest:
                    data.Add(message.ReadInt32());
                    data.Add(message.ReadBoolean());
                    break;
                case NetMessageType.GameResponse:
                    data.Add(message.ReadBoolean());
                    break;
                case NetMessageType.Name:
                    data.Add(message.ReadString());
                    break;
                case NetMessageType.PlayerMove:
                    int row = message.ReadInt32();
                    int col = message.ReadInt32();
                    data.Add(new Move(row, col));
                    break;
               case NetMessageType.Leave:
                    break;
            }
            return new Packet(type, token, data.ToArray());
        }
    }
}
