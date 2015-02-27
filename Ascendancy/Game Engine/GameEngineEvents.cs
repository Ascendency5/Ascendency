namespace Ascendancy.Game_Engine
{
    public class GameEngineEvents
    {
        public delegate void GameStateHandler(object gameEngine, GameBoardEventArgs eventArgs);

        public delegate void PlayerChangedHandler(object gameEngine, PlayerEventArgs eventArgs);

        public delegate void PlayerMovedHandler(object gameEngine, PlayerMoveEventArgs eventArgs);
    }
}
