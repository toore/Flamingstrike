using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public class EndTurnPhase : IEndTurnPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;

        public EndTurnPhase(
            IGamePhaseConductor gamePhaseConductor,
            PlayerName currentPlayerName,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayer> players,
            IDeck deck)
        {
            _gamePhaseConductor = gamePhaseConductor;
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            Deck = deck;
        }

        public PlayerName CurrentPlayerName { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public IDeck Deck { get; }

        public void EndTurn()
        {
            _gamePhaseConductor.PassTurnToNextPlayer();
        }
    }
}