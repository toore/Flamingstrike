using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface ISendArmiesToOccupyPhase
    {
        IRegion AttackingRegion { get; }
        IRegion OccupiedRegion { get; }
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
    }

    public class SendArmiesToOccupyPhase : ISendArmiesToOccupyPhase
    {
        private readonly ISendArmiesToOccupyGameState _sendArmiesToOccupyGameState;

        public SendArmiesToOccupyPhase(ISendArmiesToOccupyGameState sendArmiesToOccupyGameState)
        {
            _sendArmiesToOccupyGameState = sendArmiesToOccupyGameState;
        }

        public IRegion AttackingRegion => _sendArmiesToOccupyGameState.AttackingRegion;

        public IRegion OccupiedRegion => _sendArmiesToOccupyGameState.OccupiedRegion;

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            _sendArmiesToOccupyGameState.SendAdditionalArmiesToOccupy(numberOfArmies);
        }
    }
}