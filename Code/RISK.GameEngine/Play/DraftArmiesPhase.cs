using System.Collections.Generic;
using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface IDraftArmiesPhase : IGameStatus
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

        public IPlayer Player => _draftArmiesPhaseGameState.Player;

        public IReadOnlyList<ITerritory> Territories => _draftArmiesPhaseGameState.Territories;
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas => _draftArmiesPhaseGameState.Players;

        public int NumberOfArmiesToDraft => _draftArmiesPhaseGameState.NumberOfArmiesToDraft;

        public IReadOnlyList<IRegion> RegionsAllowedToDraftArmies { get; }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            _draftArmiesPhaseGameState.PlaceDraftArmies(region, numberOfArmies);
        }
    }
}