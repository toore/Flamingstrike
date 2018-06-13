using System.Collections.Generic;
using System.Linq;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Adapter
{
    public class DraftArmiesPhaseAdapter : IDraftArmiesPhase
    {
        private readonly GameEngine.Play.IDraftArmiesPhase _draftArmiesPhase;

        public DraftArmiesPhaseAdapter(GameEngine.Play.IDraftArmiesPhase draftArmiesPhase)
        {
            _draftArmiesPhase = draftArmiesPhase;
        }

        public string CurrentPlayerName => (string)_draftArmiesPhase.CurrentPlayerName;

        public IReadOnlyList<Territory> Territories => _draftArmiesPhase.Territories.Select(x => x.MapFromEngine()).ToList();

        public IReadOnlyList<Player> Players => _draftArmiesPhase.Players.Select(x => x.MapFromEngine()).ToList();

        public int NumberOfArmiesToDraft => _draftArmiesPhase.NumberOfArmiesToDraft;

        public IReadOnlyList<Region> RegionsAllowedToDraftArmies => _draftArmiesPhase.RegionsAllowedToDraftArmies.Select(x => x.MapFromEngine()).ToList();

        public void PlaceDraftArmies(Region region, int numberOfArmies)
        {
            _draftArmiesPhase.PlaceDraftArmies(region.MapToEngine(), numberOfArmies);
        }
    }
}