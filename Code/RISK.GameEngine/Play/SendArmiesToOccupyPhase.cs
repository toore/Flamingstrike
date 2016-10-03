using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface ISendArmiesToOccupyPhase
    {
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
        IRegion AttackingRegion { get; }
        IRegion OccupiedRegion { get; }
    }

    public class SendArmiesToOccupyPhase : ISendArmiesToOccupyPhase
    {
        private readonly ISendArmiesToOccupyGameState _sendArmiesToOccupyGameState;

        public SendArmiesToOccupyPhase(
            ISendArmiesToOccupyGameState sendArmiesToOccupyGameState,
            IRegion attackingRegion,
            IRegion occupiedRegion)
        {
            AttackingRegion = attackingRegion;
            OccupiedRegion = occupiedRegion;
            _sendArmiesToOccupyGameState = sendArmiesToOccupyGameState;
        }

        public IRegion AttackingRegion { get; }

        public IRegion OccupiedRegion { get; }

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            _sendArmiesToOccupyGameState.SendAdditionalArmiesToOccupy(numberOfArmies);
        }
    }
}