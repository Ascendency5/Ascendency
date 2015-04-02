using System.Threading;

namespace Ascendancy.Game_Engine
{
    public class GameEngine
    {
        private GameState gameState;
        private Board board;
        private BoardState boardState;

        private readonly Player redPlayer;
        private readonly Player blackPlayer;

        private Thread gameEngineThread;

        public event GameEngineEvents.GameStateHandler OnGameStart;
        public event GameEngineEvents.PlayerChangedHandler OnPlayerChanged;
        public event GameEngineEvents.PlayerMovedHandler OnPlayerMove;
        public event GameEngineEvents.GameStateHandler OnGameEnd;

        public GameEngine(Board board, Player redPlayer, Player blackPlayer, PieceType first)
        {
            this.board = board;
            gameState = GameState.Start;

            this.redPlayer = redPlayer;
            this.blackPlayer = blackPlayer;

            if (first == PieceType.Red)
                boardState = new BoardState(PieceType.Red);
            else
                boardState = new BoardState(PieceType.Black);
        }

        public void start()
        {
            gameEngineThread = new Thread(run) {IsBackground = true};
            gameEngineThread.Start();
        }

        private void run()
        {
            while (gameState != GameState.End)
            {
                if (gameState == GameState.Start)
                {
                    HandleStart();
                }
                else if(gameState == GameState.RedToMove)
                {
                    // Ask the player for a move
                    Move move = redPlayer.getMove(board, boardState);
                    boardState = boardState.PlayMove(move);

                    // Fire the event player moved
                    if (OnPlayerMove != null)
                    {
                        PlayerMoveEventArgs playerMoveEventArgs = new PlayerMoveEventArgs(board, boardState, redPlayer, move);
                        OnPlayerMove(this, playerMoveEventArgs);
                    }

                    gameState = GameState.RedMoved;
                }
                else if (gameState == GameState.RedMoved)
                {
                    // Check if there are any possible moves
                    Move[] moves = board.GetPossibleMoves(boardState);

                    if (moves.Length == 0)
                    {
                        if (OnGameEnd != null)
                        {
                            GameBoardEventArgs gameBoardEventArgs = new GameBoardEventArgs(board, boardState);
                            OnGameEnd(this, gameBoardEventArgs);
                        }
                        gameState = GameState.End;
                    }
                    else
                    {
                        if (OnPlayerChanged != null)
                        {
                            PlayerEventArgs playerEventArgs = new PlayerEventArgs(board, boardState, blackPlayer);
                            OnPlayerChanged(this, playerEventArgs);
                        }
                        gameState = GameState.BlackToMove;
                    }
                }
                else if (gameState == GameState.BlackToMove)
                {
                    // Ask the player for a move
                    Move move = blackPlayer.getMove(board, boardState);
                    boardState = boardState.PlayMove(move);

                    // Fire the event player moved
                    if (OnPlayerMove != null)
                    {
                        PlayerMoveEventArgs playerMoveEventArgs = new PlayerMoveEventArgs(board, boardState, blackPlayer, move);
                        OnPlayerMove(this, playerMoveEventArgs);
                    }
                    gameState = GameState.BlackMoved;
                }
                else if (gameState == GameState.BlackMoved)
                {
                    // Check if there are any possible moves
                    Move[] moves = board.GetPossibleMoves(boardState);

                    if (moves.Length == 0)
                    {
                        if (OnGameEnd != null)
                        {
                            GameBoardEventArgs gameBoardEventArgs = new GameBoardEventArgs(board, boardState);
                            OnGameEnd(this, gameBoardEventArgs);
                        }
                        gameState = GameState.End;
                    }
                    else
                    {
                        if (OnPlayerChanged != null)
                        {
                            PlayerEventArgs playerEventArgs = new PlayerEventArgs(board, boardState, redPlayer);
                            OnPlayerChanged(this, playerEventArgs);
                        }
                        gameState = GameState.RedToMove;
                    }
                }
            }
        }

        private void HandleStart()
        {
            // Fire start event
            if (OnGameStart != null)
            {
                GameBoardEventArgs gameBoardEventArgs = new GameBoardEventArgs(board, boardState);
                OnGameStart(this, gameBoardEventArgs);
            }

            if (boardState.CurrentPlayer == PieceType.Red)
            {
                // Red player is first
                gameState = GameState.RedToMove;
            }
            else
            {
                // Black player is first
                gameState = GameState.BlackToMove;
            }

            // Fire player event
            if (OnPlayerChanged != null)
            {
                if (boardState.CurrentPlayer == PieceType.Red)
                {
                    PlayerEventArgs playerEventArgs = new PlayerEventArgs(board, boardState, redPlayer);
                    OnPlayerChanged(this, playerEventArgs);
                }
                else
                {
                    PlayerEventArgs playerEventArgs = new PlayerEventArgs(board, boardState, blackPlayer);
                    OnPlayerChanged(this, playerEventArgs);
                }
            }
        }

        public Board GetBoard()
        {
            return board;
        }

        public void kill()
        {
            gameEngineThread.Abort();
        }
    }
}
