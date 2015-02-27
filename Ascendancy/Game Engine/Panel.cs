namespace Ascendancy.Game_Engine
{
    public struct Panel
    {
        public readonly PanelType Type;
        public readonly int TopLeftRow;
        public readonly int TopLeftCol;

        public int Score
        {
            get { return Type.GetRows()*Type.GetColumns(); }
        }

        public Panel(int row, int col, PanelType type)
        {
            this.Type = type;
            this.TopLeftRow = row;
            this.TopLeftCol = col;
        }

        public static bool operator==(Panel lhs, Panel rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Panel lhs, Panel rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Panel)) return false;
            Panel panel = (Panel) obj;
            return panel.Type == Type &&
                   panel.TopLeftRow == TopLeftRow &&
                   panel.TopLeftCol == TopLeftCol;
        }

        public override int GetHashCode()
        {
            int hash = Type.GetHashCode();
            hash = hash*31 + TopLeftRow.GetHashCode();
            hash = hash*31 + TopLeftCol.GetHashCode();
            return hash;
        }
    }
}
