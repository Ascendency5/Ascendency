using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Ascendancy.Game_Engine
{
    class AIPlayer : Player
    {
        private MCTSNode root;
        public Move getMove(Board board, BoardState state)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (root == null)
            {
                root = new MCTSNode(board, state);
            }
            else
            {
                // First pick the move we selected, then the move they selected to be the new root
                MCTSNode child = root.Children.Single(x => state.MoveBeforeLast == x.State.LastMove);
                if(child.IsLeaf) child.Expand(root.State.CurrentPlayer);
                root = child.Children.Single(x => state.LastMove == x.State.LastMove);
                root.Orphan();
            }

            while(stopwatch.ElapsedMilliseconds < 5800)
            //while(!root.IsTerminal)
            {
                if (root.IsTerminal)
                     break;
                MCTSNode current = root.RecursiveSelect();
                if (current != null)
                {
                    current.Expand(root.State.CurrentPlayer);
                }
                else
                {
                    break;
                }
            }
            /*
            Console.WriteLine("Terminal search finished in {0} ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Total possible moves: {0}", root.RecursiveChildCount());

            if (root.IsTerminal)
            {
                Console.WriteLine("Root node is terminal");
                Console.WriteLine(root);
            }
            */

            return root.bestMove();
        }
    }

    class MCTSNode
    {
        private static Random random = new Random();

        private readonly Board board;

        public BoardState State { get; private set; }

        public bool IsTerminal { get; private set; }

        public MCTSNode Parent { get; private set; }
        public MCTSNode[] Children { get; private set; }

        public int Visits { get; private set; }
        public double Score { get; private set; }

        public MCTSNode(Board board, BoardState state) :
            this(null, board, state)
        {}

        private MCTSNode(MCTSNode parent, Board board, BoardState state)
        {
            this.board = board;
            State = state;
            Visits = 0;
            Score = 0;
            Parent = parent;

            IsTerminal = board.GetPossibleMoves(state).Length == 0;
        }

        public override string ToString()
        {
            return toString("");
        }

        public string toString(string indent)
        {
            string result = indent + State.LastMove + ": " +
                   "Visits: " + Visits + " " +
                   "Score: " + Score + " " +
                   "Terminal: " + IsTerminal + " " + 
                   "Player: " + State.CurrentPlayer + "\n";
            if(!IsLeaf)
                result = Children.Aggregate(result, (current, child) => current + child.toString(indent + ' '));
            return result;
        }

        public void Orphan()
        {
            Parent = null;
        }

        public bool IsLeaf
        {
            get { return Children == null || Children.Length == 0; }
        }

        public void Expand(PieceType currentPieceType)
        {
            Children = board.GetPossibleMoves(State).Select(
                x => new MCTSNode(this, board,
                    State.PlayMove(x)
                    )
                ).ToArray();

            bool allTerminal = true;
            double totalValue = 0;

            foreach (MCTSNode child in Children)
            {
                child.Visits = 1;
                double value;
                if (child.IsTerminal)
                {
                    value = child.Score = child.GetTerminalScore(currentPieceType);
                }
                else
                {
                    allTerminal = false;
                    value = child.Score = child.Simulate(currentPieceType);
                }
                totalValue += value;
            }

            if (allTerminal)
            {
                IsTerminal = true;
                UpdateTerminal();
            }
            else
            {
                UpdateValue(totalValue, Children.Length);
            }
        }

        private void UpdateTerminal()
        {
            IsTerminal = Children.All(x => x.IsTerminal);
            Score = Children.Sum(x => x.Score);
            Visits = Children.Sum(x => x.Visits);

            if(Parent != null)
                Parent.UpdateTerminal();
        }

        public double Simulate(PieceType currentPieceType)
        {
            BoardState currentState = State;

            Move[] moves = board.GetPossibleMoves(State);
            while (moves.Length != 0)
            {
                Move randomMove = moves[random.Next(moves.Length)];
                currentState = currentState.PlayMove(randomMove);
                moves = board.GetPossibleMoves(currentState);
            }

            return new MCTSNode(board, currentState).GetTerminalScore(currentPieceType);
        }

        private double GetTerminalScore(PieceType currentPieceType)
        {

            int redScore, blackScore;
            board.GetScore(State, out redScore, out blackScore);

            if (currentPieceType == PieceType.Red)
            {
                if (redScore > blackScore)
                {
                    return 1;
                }
                if (blackScore > redScore)
                {
                    return -1;
                }
            }
            if (currentPieceType == PieceType.Black)
            {
                if (blackScore > redScore)
                {
                    return 1;
                }
                if (redScore > blackScore)
                {
                    return -1;
                }
            }

            // We tied
            return 0;
        }

        public MCTSNode Select()
        {
            return Select(x => true);
        }

        public MCTSNode Select(Func<MCTSNode, Boolean> function )
        {
            MCTSNode minNode = null;
            double bestValue = Double.MinValue;
            foreach (MCTSNode child in Children.Where(function))
            {
                double uctValue = child.UCTValue();
                if (uctValue > bestValue)
                {
                    minNode = child;
                    bestValue = uctValue;
                }
            }
            return minNode;
        }

        public double UCTValue()
        {
            return (Score/Visits) +
                   5*Math.Sqrt(Math.Log(Visits + 1)/Visits);
        }

        public void UpdateValue(double value, int numVisits)
        {
            Visits += numVisits;
            Score += value;

            if(Parent != null)
                Parent.UpdateValue(value, numVisits);
        }

        public MCTSNode RecursiveSelect()
        {
            MCTSNode current = this;
            while (!current.IsLeaf)
            {
                current = current.Select(x => !x.IsTerminal);
            }
            return current;
        }

        public int RecursiveChildCount()
        {
            if (IsLeaf) return 1;
            return Children.Sum(x => x.RecursiveChildCount());
        }

        public Move bestMove()
        {
            MCTSNode minNode = null;
            double bestValue = Double.MinValue;
            foreach (MCTSNode child in Children)
            {
                double uctValue = child.Score;
                if (uctValue > bestValue)
                {
                    minNode = child;
                    bestValue = uctValue;
                }
            }
            return minNode.State.LastMove;
        }
    }
}
