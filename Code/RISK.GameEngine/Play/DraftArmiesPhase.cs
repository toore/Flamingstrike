using System.Collections.Generic;
using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface IDraftArmiesPhase
    {
        int NumberOfArmiesToDraft { get; }
        IReadOnlyList<IRegion> RegionsAllowedToDraftArmies { get; }
        void PlaceDraftArmies(IRegion region, int numberOfArmies);
    }

    public class DraftArmiesPhase : IDraftArmiesPhase
    {
        private readonly IDraftArmiesPhaseGameState _draftArmiesPhaseGameState;

        public DraftArmiesPhase(IDraftArmiesPhaseGameState draftArmiesPhaseGameState, IPlayer currentPlayer, IReadOnlyList<ITerritory> territories, int numberOfArmiesToDraft, IReadOnlyList<IRegion> regionsAllowedToDraftArmies)
        {
            _draftArmiesPhaseGameState = draftArmiesPhaseGameState;
            CurrentPlayer = currentPlayer;
            Territories = territories;
            NumberOfArmiesToDraft = numberOfArmiesToDraft;
            RegionsAllowedToDraftArmies = regionsAllowedToDraftArmies;
        }

        public IPlayer CurrentPlayer { get; }

        public IReadOnlyList<ITerritory> Territories { get; }

        public int NumberOfArmiesToDraft { get; }

        public IReadOnlyList<IRegion> RegionsAllowedToDraftArmies { get; }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            _draftArmiesPhaseGameState.PlaceDraftArmies(region, numberOfArmies);
        }
    }
}