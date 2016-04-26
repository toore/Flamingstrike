namespace RISK.Application.Play.GamePhases
{
    public class SendArmiesToOccupyGameState : GameStateBase
    {
        public SendArmiesToOccupyGameState(IGameStateConductor gameStateConductor, GameData gameData, ConqueringAchievement conqueringAchievement)
            : base(gameData) {}
    }
}