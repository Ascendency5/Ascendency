using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascendancy.Game_Engine
{
    public struct BoardState
    {
        private PieceType currentPieceType;
        private Move lastMove;
        private Move moveBeforeLast;

        private int redMovesLeft;
        private int blackMovesLeft;

        private PieceType[,] pieces;

        public BoardState(PieceType type)
        {
            currentPieceType = type;
            lastMove = Move.None;
            moveBeforeLast = Move.None;

            redMovesLeft = 28;
            blackMovesLeft = 28;

            pieces = new PieceType[8,8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pieces[i, j] = PieceType.Empty;
                }
            }
        }

        private BoardState(BoardState oldState)
        {
            currentPieceType = oldState.currentPieceType;
            lastMove = oldState.lastMove;
            moveBeforeLast = oldState.moveBeforeLast;

            redMovesLeft = oldState.redMovesLeft;
            blackMovesLeft = oldState.blackMovesLeft;

            pieces = new PieceType[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pieces[i, j] = oldState.pieces[i, j];
                }
            }
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

        public Move LastMove
        {
            get { return lastMove; }
        }

        public Move MoveBeforeLast
        {
            get { return moveBeforeLast; }
        }

        public int BlackMovesLeft
        {
            get { return blackMovesLeft; }
        }

        public int RedMovesLeft
        {
            get { return redMovesLeft; }
        }

        public PieceType CurrentPlayer
        {
            get { return currentPieceType; }
        }

        public BoardState PlayMove(Move move)
        {
            BoardState newState = new BoardState(this);
            if (currentPieceType == PieceType.Red)
            {
                newState.currentPieceType = PieceType.Black;
                newState.redMovesLeft--;
            }
            else if (currentPieceType == PieceType.Black)
            {
                newState.currentPieceType = PieceType.Red;
                newState.blackMovesLeft--;
            }

            newState.pieces[move.Row, move.Col] = currentPieceType;
            newState.moveBeforeLast = lastMove;
            newState.lastMove = move;

            return newState;
        }
    }
}
