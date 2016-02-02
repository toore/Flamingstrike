namespace RISK.Application.Play.GamePhases
{
    public class DraftArmiesGameState : GameStateBase
    {
        private readonly GameStateFactory _gameStateFactory;
        private readonly int _numberOfArmiesToDraft;

        public DraftArmiesGameState(GameStateFactory gameStateFactory, GameData gameData, int numberOfArmiesToDraft)
            : base(gameData)
        {
            _gameStateFactory = gameStateFactory;
            _numberOfArmiesToDraft = numberOfArmiesToDraft;
        }

        public override int GetNumberOfArmiesToDraft()
        {
            return _numberOfArmiesToDraft;
        }
    }
}