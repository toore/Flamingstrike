using System.Collections.Generic;
using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface IDraftArmiesPhase
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        int NumberOfArmiesToDraft { get; }
        IReadOnlyList<IRegion> RegionsAllowedToDraftArmies { get; }
        void PlaceDraftArmies(IRegion region, int numberOfArmies);
    }

    public class DraftArmiesPhase : IDraftArmiesPhase
    {
        private readonly IDraftArmiesPhaseGameState _draftArmiesPhaseGameState;

        public DraftArmiesPhase(
            IPlayer currentPlayer, 
            IReadOnlyList<ITerritory> territories, 
            IReadOnlyList<IPlayerGameData> playerGameDatas,
            IDraftArmiesPhaseGameState draftArmiesPhaseGameState, 
            IReadOnlyList<IRegion> regionsAllowedToDraftArmies)
        {
            CurrentPlayer = currentPlayer;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
            _draftArmiesPhaseGameState = draftArmiesPhaseGameState;
            RegionsAllowedToDraftArmies = regionsAllowedToDraftArmies;
        }

        public IPlayer CurrentPlayer { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        public int NumberOfArmiesToDraft => _draftArmiesPhaseGameState.NumberOfArmiesToDraft;

        public IReadOnlyList<IRegion> RegionsAllowedToDraftArmies { get; }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            _draftArmiesPhaseGameState.PlaceDraftArmies(region, numberOfArmies);
        }
    }
}