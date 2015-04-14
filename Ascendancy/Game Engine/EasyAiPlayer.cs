using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ascendancy.Game_Engine
{
    public class EasyAiPlayer : Player
    {
        private string Name;
        private Random random;

        public EasyAiPlayer() : this("Robots")
        {
        }

        public EasyAiPlayer(string name)
        {
            Name = name;
            random = new Random();
        }

        public Move GetMove(Board board, BoardState state)
        {
            Move[] moves = board.GetPossibleMoves(state);

            Thread.Sleep(3500);

            return moves[random.Next(moves.Length)];
        }

        public string GetName()
        {
            return Name;
        }

        public void Reset()
        {
            // Nothing to do to reset this
        }
    }
}
