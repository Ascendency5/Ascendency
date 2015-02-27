namespace Ascendancy.Game_Engine
{
    public class HumanPlayer : Player
    {
        public delegate Move HumanMoveHandler(object humanPlayer, GameBoardEventArgs args);

        private event HumanMoveHandler OnMoveNeeded;

        public HumanPlayer(HumanMoveHandler handler)
        {
            OnMoveNeeded += handler;
        }

        public Move getMove(Board board, BoardState state)
        {
            return OnMoveNeeded(this, new GameBoardEventArgs(board, state));
        }
    }
}
