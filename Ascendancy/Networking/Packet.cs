using System;
using System.Linq;
using Lidgren.Network;

namespace Ascendancy.Networking
{
    public class Packet
    {
        public NetMessageType Type { get; private set; }
        public Guid Token { get; private set; }
        public object[] Data { get; private set; }

        public static Packet Create(NetMessageType type, params object[] data)
        {
            return new Packet(type, Guid.NewGuid(), data);
        }
        
        public Packet(NetMessageType type, Guid token, params object[] data)
        {
            Token = token;
            Type = type;
            if (data == null) return;

            Data = new object[data.Length];
            Array.Copy(data, Data, data.Length);
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
