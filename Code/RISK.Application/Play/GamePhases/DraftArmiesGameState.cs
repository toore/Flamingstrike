namespace RISK.Application.Play.GamePhases
{
    public class DraftArmiesGameState : GameStateBase
    {
        private readonly GameStateFactory _gameStateFactory;

        public DraftArmiesGameState(GameStateFactory gameStateFactory, GameData gameData)
            : base(gameData)
        {
            _gameStateFactory = gameStateFactory;
        }
    }
}