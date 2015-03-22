using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public class Packet
    {
        public Networkmanager.MessageType Type { get; private set; }
        public object[] Data { get; private set; }

        public Packet(Networkmanager.MessageType type, params object[] data)
        {
            Type = type;
            if (data != null)
            {
                Data = new object[data.Length];
                Array.Copy(data, Data, data.Length);
            }
        }

        public static Packet[] Read(NetIncomingMessage message)
        {
            List<Packet> packets = new List<Packet>();

            int packetCount = message.ReadInt32();
            for (int i = 0; i < packetCount; i++)
            {
                Networkmanager.MessageType type = (Networkmanager.MessageType)message.ReadInt32();
                List<object> data = new List<object>();
                switch (type)
                {
                    case Networkmanager.MessageType.Identifier:
                        data.Add(message.ReadString());
                        break;
                    case Networkmanager.MessageType.Chat:
                        data.Add(message.ReadString());
                        break;
                    case Networkmanager.MessageType.GameRequest:
                        data.Add(message.ReadInt32());
                        data.Add(message.ReadBoolean());
                        break;
                    case Networkmanager.MessageType.GameResponse:
                        data.Add(message.ReadBoolean());
                        break;
                    case Networkmanager.MessageType.Name:
                        data.Add(message.ReadString());
                        break;
                    case Networkmanager.MessageType.PlayerMove:
                        data.Add(message.ReadInt32());
                        data.Add(message.ReadInt32());
                        break;
                }

                packets.Add(new Packet(type, data.ToArray()));
            }

            return packets.ToArray<Packet>();
        }

        public override string ToString()
        {
            string result = Type.ToString();
            result += ": ";

            foreach (var dat in Data)
            {
                result += dat.ToString() + " ";
            }

            return result;
        }
    }
}
