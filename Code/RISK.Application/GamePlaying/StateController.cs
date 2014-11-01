namespace RISK.Domain.GamePlaying
{
    public class StateController
    {
        public IInteractionState CurrentState { get; set; }
        public bool PlayerShouldReceiveCardWhenTurnEnds { get; set; }
    }
}