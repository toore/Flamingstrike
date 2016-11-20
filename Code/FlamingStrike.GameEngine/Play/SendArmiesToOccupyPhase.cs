using System.Collections.Generic;
using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface ISendArmiesToOccupyPhase : IGameStatus
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

        public IPlayer Player => _sendArmiesToOccupyGameState.Player;

        public IReadOnlyList<ITerritory> Territories => _sendArmiesToOccupyGameState.Territories;
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas => _sendArmiesToOccupyGameState.Players;

        public IRegion AttackingRegion => _sendArmiesToOccupyGameState.AttackingRegion;

        public IRegion OccupiedRegion => _sendArmiesToOccupyGameState.OccupiedRegion;

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            _sendArmiesToOccupyGameState.SendAdditionalArmiesToOccupy(numberOfArmies);
        }
    }
}