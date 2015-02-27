using System;
using System.Collections.Generic;
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
                root = child.Children.Single(x => state.LastMove == x.State.LastMove);
            }

            while(stopwatch.ElapsedMilliseconds < 5800)
            {
                RunMCTS();
            }

            return root.Select().State.LastMove;
        }

        private void RunMCTS()
        {
            List<MCTSNode> visitedNodes = new List<MCTSNode> {root};

            MCTSNode current = root;

            // Selection
            while (!current.IsLeaf)
            {
                current = current.Select();
                visitedNodes.Add(current);
            }

            // Expansion
            current.Expand();

            MCTSNode newNode;
            if (current.Children.Length != 0)
            {
                newNode = current.Select();
                visitedNodes.Add(newNode);
            }
            else
            {
                // todo Figure out what's actually correct here
                newNode = current;
            }

            // Simulation
            double value = newNode.Simulate(root.State.CurrentPlayer);
            
            // Bakcpropagation
            foreach (MCTSNode node in visitedNodes)
            {
                node.UpdateValue(value);
            }
        }
    }

    class MCTSNode
    {
        private static Random random = new Random();

        private readonly Board board;
        private BoardState state;

        public BoardState State
        {
            get { return state; }
        }

        public MCTSNode[] Children { get; private set; }

        public int Visits { get; private set; }

        public double Score { get; private set; }

        public MCTSNode(Board board, BoardState state)
        {
            this.board = board;
            this.state = state;
            this.Visits = 0;
            this.Score = 0;
        }

        public bool IsLeaf
        {
            get { return Children == null || Children.Length == 0; }
        }

        public void Expand()
        {
            // todo Make the function only select unvisited nodes at first
            Children = board.GetPossibleMoves(state).Select(
                x => new MCTSNode(board,
                    state.PlayMove(x)
                    )
                ).ToArray();
        }

        public double Simulate(PieceType currentPieceType)
        {
            BoardState currentState = state;

            Move[] moves = board.GetPossibleMoves(state);
            while (moves.Length != 0)
            {
                Move randomMove = moves[random.Next(moves.Length)];
                currentState = currentState.PlayMove(randomMove);
                moves = board.GetPossibleMoves(currentState);
            }

            int redScore, blackScore;
            board.GetScore(state, out redScore, out blackScore);

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

            // We tied, which counts as a loss
            return 0;
        }

        public MCTSNode Select()
        {
            MCTSNode minNode = null;
            double bestValue = Double.MinValue;
            foreach (MCTSNode child in Children)
            {
                double uctValue = child.Score / (child.Visits + Double.Epsilon) +
                               Math.Sqrt(Math.Log10(Visits + 1) / (child.Visits + Double.Epsilon)) +
                               random.NextDouble()*Double.Epsilon;
                if (uctValue > bestValue)
                {
                    minNode = child;
                    bestValue = uctValue;
                }
            }
            return minNode;
        }

        public void UpdateValue(double value)
        {
            Visits++;
            Score += value;
        }
    }
}
