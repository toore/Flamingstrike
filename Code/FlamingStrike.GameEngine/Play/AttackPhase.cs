using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public class AttackPhase : IAttackPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private readonly IWorldMap _worldMap;
        private ConqueringAchievement _conqueringAchievement;

        public AttackPhase(
            IGamePhaseConductor gamePhaseConductor,
            PlayerName currentPlayerName,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayer> players,
            IDeck deck,
            ConqueringAchievement conqueringAchievement,
            IAttacker attacker,
            IFortifier fortifier,
            IPlayerEliminationRules playerEliminationRules,
            IWorldMap worldMap)
        {
            _gamePhaseConductor = gamePhaseConductor;
            _conqueringAchievement = conqueringAchievement;
            _attacker = attacker;
            _fortifier = fortifier;
            _playerEliminationRules = playerEliminationRules;
            _worldMap = worldMap;
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            Deck = deck;
        }

        public PlayerName CurrentPlayerName { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public IDeck Deck { get; }

        public IReadOnlyList<Region> GetRegionsThatCanBeSourceForAttackOrFortification()
        {
            return Territories
                .Where(x => IsCurrentPlayerOccupyingRegion(x.Region))
                .Select(x => x.Region)
                .ToList();
        }

        public void Attack(Region attackingRegion, Region defendingRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(attackingRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(attackingRegion)}.");
            }

            var defendingPlayerBeforeTerritoriesAreUpdated = GetPlayer(defendingRegion);

            var attackOutcome = _attacker.Attack(Territories, attackingRegion, defendingRegion);

            if (attackOutcome.DefendingArmyAvailability == DefendingArmyAvailability.IsEliminated)
            {
                var defeatedPlayer = defendingPlayerBeforeTerritoriesAreUpdated;
                DefendingArmyIsEliminated(defeatedPlayer, attackingRegion, defendingRegion, attackOutcome.Territories);
            }
            else
            {
                MoveOnToAttackPhase(attackOutcome.Territories, Players);
            }
        }

        public void Fortify(Region sourceRegion, Region destinationRegion, int armies)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(sourceRegion)}.");
            }

            var updatedTerritories = _fortifier.Fortify(Territories, sourceRegion, destinationRegion, armies);
            var playersAndDeck = UpdatePlayersAndDeck();
            var updatedGameData = new GameData(updatedTerritories, playersAndDeck.Players, CurrentPlayerName, playersAndDeck.Deck);

            _gamePhaseConductor.WaitForTurnToEnd(updatedGameData);
        }

        public IEnumerable<Region> GetRegionsThatCanBeAttacked(Region sourceRegion)
        {
            return _worldMap.GetBorders(sourceRegion)
                .Where(borderRegion => CanAttack(sourceRegion, borderRegion));
        }

        public IEnumerable<Region> GetRegionsThatCanBeFortified(Region sourceRegion)
        {
            return _worldMap.GetBorders(sourceRegion)
                .Where(borderRegion => CanFortify(sourceRegion, borderRegion));
        }

        public void EndTurn()
        {
            var playersAndDeck = UpdatePlayersAndDeck();

            var updatedGameData = new GameData(Territories, playersAndDeck.Players, CurrentPlayerName, playersAndDeck.Deck);

            _gamePhaseConductor.PassTurnToNextPlayer(updatedGameData);
        }

        private bool IsCurrentPlayerOccupyingRegion(Region region)
        {
            return Territories.Single(x => x.Region == region).Name == CurrentPlayerName;
        }

        private PlayerName GetPlayer(Region region)
        {
            return Territories.Single(x => x.Region == region).Name;
        }

        private void DefendingArmyIsEliminated(PlayerName defeatedPlayerName, Region attackingRegion, Region defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _conqueringAchievement = ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            if (_playerEliminationRules.IsOnePlayerLeftInTheGame(updatedTerritories))
            {
                GameIsOver();
            }
            else if (_playerEliminationRules.IsPlayerEliminated(updatedTerritories, defeatedPlayerName))
            {
                AquireAllCardsFromPlayerAndSendArmiesToOccupy(defeatedPlayerName, attackingRegion, defeatedRegion, updatedTerritories);
            }
            else
            {
                ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion, updatedTerritories, Players);
            }
        }

        private void GameIsOver()
        {
            _gamePhaseConductor.PlayerIsTheWinner(CurrentPlayerName);
        }

        private void AquireAllCardsFromPlayerAndSendArmiesToOccupy(PlayerName defeatedPlayerName, Region attackingRegion, Region defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            var defeatedPlayer = Players.Single(x => x.Name == defeatedPlayerName);
            var attackingPlayer = Players.Single(x => x.Name == CurrentPlayerName);

            defeatedPlayer.EliminatedBy(attackingPlayer);

            ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion, updatedTerritories, Players);
        }

        private IPlayer GetCurrentPlayer()
        {
            return Players.Single(x => x.Name == CurrentPlayerName);
        }

        private void ContinueWithAttackOrOccupation(Region sourceRegion, Region destinationRegion, IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayer> updatedPlayers)
        {
            var numberOfArmiesThatCanBeSentToOccupy = updatedTerritories
                .Single(x => x.Region == sourceRegion).GetNumberOfArmiesThatCanBeSentToOccupy();

            if (numberOfArmiesThatCanBeSentToOccupy > 0)
            {
                SendArmiesToOccupy(sourceRegion, destinationRegion, updatedTerritories, updatedPlayers);
            }
            else
            {
                MoveOnToAttackPhase(updatedTerritories, updatedPlayers);
            }
        }

        private void SendArmiesToOccupy(Region sourceRegion, Region destinationRegion, IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayer> updatedPlayerGameDatas)
        {
            var updatedGameData = new GameData(updatedTerritories, updatedPlayerGameDatas, CurrentPlayerName, Deck);

            _gamePhaseConductor.SendArmiesToOccupy(sourceRegion, destinationRegion, updatedGameData);
        }

        private void MoveOnToAttackPhase(IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayer> updatedPlayerGameDatas)
        {
            var updatedGameData = new GameData(updatedTerritories, updatedPlayerGameDatas, CurrentPlayerName, Deck);

            _gamePhaseConductor.ContinueWithAttackPhase(_conqueringAchievement, updatedGameData);
        }

        private bool CanAttack(Region attackingRegion, Region defendingRegion)
        {
            return IsCurrentPlayerOccupyingRegion(attackingRegion)
                   &&
                   _attacker.CanAttack(Territories, attackingRegion, defendingRegion);
        }

        private bool CanFortify(Region sourceRegion, Region destinationRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                return false;
            }

            return _fortifier.CanFortify(Territories, sourceRegion, destinationRegion);
        }

        private PlayersAndDeck UpdatePlayersAndDeck()
        {
            if (ShouldCardBeAwarded())
            {
                GetCurrentPlayer().AddCard(Deck);
            }

            return new PlayersAndDeck(Players, Deck);
        }

        private bool ShouldCardBeAwarded()
        {
            return _conqueringAchievement == ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;
        }

        private class PlayersAndDeck
        {
            public IReadOnlyList<IPlayer> Players { get; }
            public IDeck Deck { get; }

            public PlayersAndDeck(IReadOnlyList<IPlayer> players, IDeck deck)
            {
                Players = players;
                Deck = deck;
            }
        }
    }
}