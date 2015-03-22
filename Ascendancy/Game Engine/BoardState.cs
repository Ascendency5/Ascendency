using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Game_Engine
{
    public struct BoardState
    {
        public PieceType CurrentPlayer { get; private set; }

        public int RedMovesLeft { get; private set; }
        public int BlackMovesLeft { get; private set; }

        private PieceType[,] pieces;

        public Move LastMove { get; private set; }
        public Move MoveBeforeLast { get; private set; }

        // this is explicitely called so properties can be used
        public BoardState(PieceType type) : this()
        {
            pieces = new PieceType[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pieces[i, j] = PieceType.Empty;
                }
            }

            CurrentPlayer = type;

            RedMovesLeft = 28;
            BlackMovesLeft = 28;

            LastMove = Move.None;
            MoveBeforeLast = Move.None;
        }

        // this is explicitely called so properties can be used
        private BoardState(BoardState oldState) : this()
        {
            pieces = new PieceType[8, 8];
            Array.Copy(oldState.pieces, pieces, oldState.pieces.Length);

            CurrentPlayer = oldState.CurrentPlayer;

            RedMovesLeft = oldState.RedMovesLeft;
            BlackMovesLeft = oldState.BlackMovesLeft;

            LastMove = oldState.LastMove;
            MoveBeforeLast = oldState.MoveBeforeLast;
        }

        public PieceType this[Move move]
        {
            get { return this[move.Row, move.Col]; }
        }

        public PieceType this[int row, int col]
        {
            get { return GetPieceType(row, col); }
        }

        public PieceType GetPieceType(int row, int col)
        {
            return pieces[row, col];
        }

        public BoardState PlayMove(Move move)
        {
            BoardState newState = new BoardState(this);
            if (CurrentPlayer == PieceType.Red)
            {
                newState.CurrentPlayer = PieceType.Black;
                newState.RedMovesLeft--;
            }
            else if (CurrentPlayer == PieceType.Black)
            {
                newState.CurrentPlayer = PieceType.Red;
                newState.BlackMovesLeft--;
            }

            newState.pieces[move.Row, move.Col] = CurrentPlayer;
            newState.MoveBeforeLast = LastMove;
            newState.LastMove = move;

            return newState;
        }
    }
}
