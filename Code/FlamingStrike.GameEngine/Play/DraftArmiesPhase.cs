using System.Collections.Generic;
using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
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

        public DraftArmiesPhase(IDraftArmiesPhaseGameState draftArmiesPhaseGameState, IReadOnlyList<IRegion> regionsAllowedToDraftArmies)
        {
            _draftArmiesPhaseGameState = draftArmiesPhaseGameState;
            RegionsAllowedToDraftArmies = regionsAllowedToDraftArmies;
        }

        public int NumberOfArmiesToDraft => _draftArmiesPhaseGameState.NumberOfArmiesToDraft;

        public IReadOnlyList<IRegion> RegionsAllowedToDraftArmies { get; }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            _draftArmiesPhaseGameState.PlaceDraftArmies(region, numberOfArmies);
        }
    }
}