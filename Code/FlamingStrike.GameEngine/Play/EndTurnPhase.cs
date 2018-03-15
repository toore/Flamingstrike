using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IEndTurnPhase
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        void EndTurn();
    }

    public class EndTurnPhase : IEndTurnPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;

        public EndTurnPhase(
            IGamePhaseConductor gamePhaseConductor,
            IPlayer currentPlayer,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayerGameData> playerGameDatas,
            IDeck deck)
        {
            _gamePhaseConductor = gamePhaseConductor;
            CurrentPlayer = currentPlayer;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
            Deck = deck;
        }

        public IPlayer CurrentPlayer { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        public IDeck Deck { get; }

        public void EndTurn()
        {
            _gamePhaseConductor.PassTurnToNextPlayer(new GameData(Territories, PlayerGameDatas, CurrentPlayer, Deck));
        }
    }
}