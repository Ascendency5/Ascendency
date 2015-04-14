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
        public string Token { get; private set; }
        public object[] Data { get; private set; }

        public Packet(Networkmanager.MessageType type, params object[] data) :
            this(Guid.NewGuid().ToString(), type, data)
        {
        }

        private Packet(string token, Networkmanager.MessageType type, params object[] data)
        {
            Token = token;
            Type = type;
            if (data == null) return;

            Data = new object[data.Length];
            Array.Copy(data, Data, data.Length);
        }

        public static Packet[] Read(NetIncomingMessage message)
        {
            List<Packet> packets = new List<Packet>();

            int packetCount = message.ReadInt32();
            for (int i = 0; i < packetCount; i++)
            {
                Networkmanager.MessageType type = (Networkmanager.MessageType)message.ReadInt32();
                string token = message.ReadString();
                List<object> data = new List<object>();
                switch (type)
                {
                    case Networkmanager.MessageType.Ack:
                        data.Add(message.ReadString());
                        break;
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

                packets.Add(new Packet(token, type, data.ToArray()));
            }

            return packets.ToArray<Packet>();
        }

        public override string ToString()
        {
            return Data.Aggregate(Type + ": ", (current, dat) => current + (dat.ToString() + " "));
        }

        private bool Equals(Packet other)
        {
            return string.Equals(Token, other.Token) && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Packet) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Token != null ? Token.GetHashCode() : 0)*397) ^ (int) Type;
            }
        }
    }
}
