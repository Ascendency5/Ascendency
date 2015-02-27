using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Game_Engine
{
    public struct Move
    {
        public readonly int Row;
        public readonly int Col;

        public static Move None
        {
            get
            {
                return new Move(-1, -1);
            }
        }

        public Move(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public override string ToString()
        {
            return "(" + Row + "," + Col + ")";
        }

        public static bool operator==(Move lhs, Move rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Move lhs, Move rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Move)) return false;
            Move move = (Move) obj;
            return move.Row == Row && move.Col == Col;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
