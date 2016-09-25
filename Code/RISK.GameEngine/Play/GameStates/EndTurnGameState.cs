namespace RISK.GameEngine.Play.GameStates
{
    public interface IEndTurnGameState
    {
        void EndTurn();
    }

    public class EndTurnGameState : IEndTurnGameState
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;

        public EndTurnGameState(IGamePhaseConductor gamePhaseConductor)
        {
            _gamePhaseConductor = gamePhaseConductor;
        }

        public void EndTurn()
        {
            _gamePhaseConductor.PassTurnToNextPlayer();
        }
    }
}