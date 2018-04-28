using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Play
{
    public class EndTurnPhaseAdapter : IEndTurnPhase
    {
        private readonly GameEngine.Play.IEndTurnPhase _endTurnPhase;

        public EndTurnPhaseAdapter(GameEngine.Play.IEndTurnPhase endTurnPhase)
        {
            _endTurnPhase = endTurnPhase;
        }

        public string CurrentPlayerName => (string)_endTurnPhase.CurrentPlayerName;
        public IReadOnlyList<Territory> Territories => _endTurnPhase.Territories.Select(x => x.MapFromEngine()).ToList();
        public IReadOnlyList<Player> Players => _endTurnPhase.Players.Select(x => x.MapFromEngine()).ToList();

        public void EndTurn()
        {
            _endTurnPhase.EndTurn();
        }
    }
}