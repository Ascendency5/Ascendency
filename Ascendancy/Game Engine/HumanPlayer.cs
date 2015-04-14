namespace Ascendancy.Game_Engine
{
    public class HumanPlayer : Player
    {
        public delegate Move HumanMoveHandler(object humanPlayer, GameBoardEventArgs args);

        private event HumanMoveHandler OnMoveNeeded;
        private string Name;

        public HumanPlayer(HumanMoveHandler handler) :
            this("Humans", handler)
        {
        }

        public HumanPlayer(string name, HumanMoveHandler handler)
        {
            this.Name = name;
            OnMoveNeeded += handler;
        }

        public Move GetMove(Board board, BoardState state)
        {
            return OnMoveNeeded(this, new GameBoardEventArgs(board, state));
        }

        public string GetName()
        {
            return Name;
        }

        public void Reset()
        {
            // Nothing needs to happen for the human to reset
        }
    }
}
