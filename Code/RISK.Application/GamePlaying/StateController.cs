namespace RISK.Domain.GamePlaying
{
    public class StateController
    {
        public ITurnState CurrentState { get; set; }
        public bool PlayerShouldReceiveCardWhenTurnEnds { get; set; }
    }
}