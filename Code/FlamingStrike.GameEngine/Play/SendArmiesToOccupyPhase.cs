using System.Collections.Generic;
using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface ISendArmiesToOccupyPhase
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        IRegion AttackingRegion { get; }
        IRegion OccupiedRegion { get; }
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
    }

    public class SendArmiesToOccupyPhase : ISendArmiesToOccupyPhase
    {
        public IPlayer CurrentPlayer { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        private readonly ISendArmiesToOccupyGameState _sendArmiesToOccupyGameState;

        public SendArmiesToOccupyPhase(IPlayer currentPlayer, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, ISendArmiesToOccupyGameState sendArmiesToOccupyGameState)
        {
            CurrentPlayer = currentPlayer;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
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