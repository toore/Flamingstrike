using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public class EndTurnInteractionState : InteractionStateBase
    {
        private readonly IEndTurnPhase _endTurnPhase;

        public EndTurnInteractionState(IEndTurnPhase endTurnPhase)
        {
            _endTurnPhase = endTurnPhase;
        }

        public override string Title => Resources.END_TURN;

        public override bool CanEndTurn => true;

        public override void EndTurn()
        {
            _endTurnPhase.EndTurn();
        }
    }
}