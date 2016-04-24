namespace RISK.Application.Play.GamePhases
{
    public class SendInArmiesToOccupyGameState : GameStateBase
    {
        public SendInArmiesToOccupyGameState(IGameStateConductor gameStateConductor, GameData gameData, ConqueringAchievement conqueringAchievement)
            : base(gameData) {}
    }
}