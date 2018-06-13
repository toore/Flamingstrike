using System.Collections.Generic;
using System.Linq;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Adapter
{
    public class SendArmiesToOccupyPhaseAdapter : ISendArmiesToOccupyPhase
    {
        private readonly GameEngine.Play.ISendArmiesToOccupyPhase _sendArmiesToOccupyPhase;

        public SendArmiesToOccupyPhaseAdapter(GameEngine.Play.ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _sendArmiesToOccupyPhase = sendArmiesToOccupyPhase;
        }

        public string CurrentPlayerName => (string)_sendArmiesToOccupyPhase.CurrentPlayerName;
        public IReadOnlyList<Territory> Territories => _sendArmiesToOccupyPhase.Territories.Select(x => x.MapFromEngine()).ToList();
        public IReadOnlyList<Player> Players => _sendArmiesToOccupyPhase.Players.Select(x => x.MapFromEngine()).ToList();
        public Region AttackingRegion => _sendArmiesToOccupyPhase.AttackingRegion.MapFromEngine();
        public Region OccupiedRegion => _sendArmiesToOccupyPhase.OccupiedRegion.MapFromEngine();

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            _sendArmiesToOccupyPhase.SendAdditionalArmiesToOccupy(numberOfArmies);
        }
    }
}