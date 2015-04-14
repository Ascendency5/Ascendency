using System;
using System.Collections;

namespace Ascendancy.Game_Engine
{

    public class Board
    {
        private Hashtable panelsToPosition;
        private Hashtable positionsToPanel;
        private int boardType;

        public Board(Panel[] panels, int boardType)
        {
            if (panels.Length != 17)
            {
                throw new Exception("17 panels must be given");
            }
            panelsToPosition = new Hashtable();
            positionsToPanel = new Hashtable();

            foreach (Panel panel in panels)
            {
                AddPanel(panel);
            }
            this.boardType = boardType;
        }

        public int GetBoardType()
        {
            return boardType;
        }

        private void AddPanel(Panel panel)
        {
            int rows = panel.Type.GetRows();
            int cols = panel.Type.GetColumns();

            ArrayList moves = new ArrayList();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Move move = new Move(panel.TopLeftRow + i, panel.TopLeftCol + j);
                    moves.Add(move);
                    positionsToPanel.Add(move, panel);
                }
            }

            panelsToPosition.Add(panel, moves);
        }

        public Panel GetPanel(Move move)
        {
            return GetPanel(move.Row, move.Col);
        }

        public Panel GetPanel(int row, int col)
        {
            return (Panel) positionsToPanel[new Move(row, col)];
        }

        public Move[] GetPossibleMoves(BoardState state)
        {
            ArrayList moves = new ArrayList();

            if(state.CurrentPlayer == PieceType.Red && state.RedMovesLeft == 0)
                return new Move[0];
            if(state.CurrentPlayer == PieceType.Black && state.BlackMovesLeft == 0)
                return new Move[0];

            // Any moves yet?
            if (state.LastMove == Move.None)
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        moves.Add(new Move(i, j));
            }
            else
            {
                // Same row
                for (int i = 0; i < 8; i++)
                {
                    if(state[state.LastMove.Row, i] == PieceType.Empty)
                        moves.Add(new Move(state.LastMove.Row, i));
                }

                // Same column
                for (int i = 0; i < 8; i++)
                {
                    if(state[i, state.LastMove.Col] == PieceType.Empty)
                        moves.Add(new Move(i, state.LastMove.Col));    
                }

                // Remove in the same as my last move
                RemoveInvalidMoves(moves, state.LastMove);

                // Remove in the same as their last move
                if (state.MoveBeforeLast != Move.None)
                {
                    RemoveInvalidMoves(moves, state.MoveBeforeLast);
                }
            }

            return (Move[]) moves.ToArray(typeof(Move));
        }

        private void RemoveInvalidMoves(ArrayList moves, Move moveToRemove)
        {
            for (int i = moves.Count - 1; i >= 0; i--)
            {
                Move move = (Move) moves[i];
                if (GetPanel(move.Row, move.Col) == GetPanel(moveToRemove.Row, moveToRemove.Col))
                {
                    moves.Remove(move);
                }
            }
        }

        public void GetScore(BoardState state, out int redScore, out int blackScore)
        {
            redScore = 0;
            blackScore = 0;

            foreach (Panel panel in panelsToPosition.Keys)
            {
                int tempRedScore;
                int tempBlackScore;
                GetScore(state, panel, out tempRedScore, out tempBlackScore);
                redScore += tempRedScore;
                blackScore += tempBlackScore;
            }
        }

        private void GetScore(BoardState state, Panel panel, out int redScore, out int blackScore)
        {
            Move[] moves = GetMovesFromPanel(panel);

            redScore = 0;
            blackScore = 0;

            foreach (Move move in moves)
            {
                if (state[move] == PieceType.Red)
                    redScore++;
                if (state[move] == PieceType.Black)
                    blackScore++;
            }

            if (redScore > blackScore)
            {
                redScore = panel.Score;
                blackScore = 0;
            }
            else if (blackScore > redScore)
            {
                redScore = 0;
                blackScore = panel.Score;
            }
            else
            {
                redScore = 0;
                blackScore = 0;
            }
        }

        private Move[] GetMovesFromPanel(Panel panel)
        {
            ArrayList moves = (ArrayList) panelsToPosition[panel];
            return (Move[]) moves.ToArray(typeof (Move));
        }
    }
}
