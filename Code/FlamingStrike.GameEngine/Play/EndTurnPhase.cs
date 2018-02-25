using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface IEndTurnPhase
    {
        void EndTurn();
    }

    public class EndTurnPhase : IEndTurnPhase
    {
        private readonly IEndTurnGameState _endTurnGameState;

        public EndTurnPhase(IEndTurnGameState endTurnGameState)
        {
            _endTurnGameState = endTurnGameState;
        }

        public void EndTurn()
        {
            _endTurnGameState.EndTurn();
        }
    }
}