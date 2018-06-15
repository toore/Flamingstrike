using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public class AttackPhase : IAttackPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttackService _attackService;
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
            IAttackService attackService,
            IPlayerEliminationRules playerEliminationRules,
            IWorldMap worldMap)
        {
            _gamePhaseConductor = gamePhaseConductor;
            _conqueringAchievement = conqueringAchievement;
            _attackService = attackService;
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

        public IEnumerable<Region> RegionsThatCanBeSourceForAttackOrFortification
        {
            get
            {
                return Territories
                    .Where(x => IsCurrentPlayerOccupyingRegion(x.Region))
                    .Select(x => x.Region);
            }
        }

        public void Attack(Region attackingRegion, Region defendingRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(attackingRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(attackingRegion)}.");
            }

            var defendingPlayerBeforeTerritoriesAreUpdated = GetPlayer(defendingRegion);

            var attackingTerritory = Territories.Single(x => x.Region == attackingRegion);
            var defendingTerritory = Territories.Single(x => x.Region == defendingRegion);
            var attackOutcome = _attackService.Attack(attackingTerritory, defendingTerritory);

            if (attackOutcome == DefendingArmyStatus.IsEliminated)
            {
                var defeatedPlayer = defendingPlayerBeforeTerritoriesAreUpdated;
                DefendingArmyIsEliminated(defeatedPlayer, attackingRegion, defendingRegion, Territories);
            }
            else
            {
                MoveOnToAttackPhase();
            }
        }

        public void Fortify(Region sourceRegion, Region destinationRegion, int armies)
        {
            if (!CanFortify(sourceRegion, destinationRegion))
            {
                throw new InvalidOperationException($"Can't fortify from {nameof(sourceRegion)} to {nameof(destinationRegion)}");
            }

            var sourceTerritory = Territories.Single(territory => territory.Region == sourceRegion);
            var destinationTerritory = Territories.Single(territory => territory.Region == destinationRegion);

            sourceTerritory.RemoveArmies(armies);
            destinationTerritory.AddArmies(armies);

            MaybeAwardCard();

            _gamePhaseConductor.WaitForTurnToEnd();
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
            MaybeAwardCard();

            _gamePhaseConductor.PassTurnToNextPlayer();
        }

        private bool IsCurrentPlayerOccupyingRegion(Region region)
        {
            return Territories.Single(x => x.Region == region).PlayerName == CurrentPlayerName;
        }

        private PlayerName GetPlayer(Region region)
        {
            return Territories.Single(x => x.Region == region).PlayerName;
        }

        private void DefendingArmyIsEliminated(PlayerName defeatedPlayerName, Region attackingRegion, Region defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _conqueringAchievement = ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            if (_playerEliminationRules.IsOnePlayerLeftInTheGame(Territories))
            {
                GameIsOver();
            }
            else if (_playerEliminationRules.IsPlayerEliminated(updatedTerritories, defeatedPlayerName))
            {
                AquireAllCardsFromPlayerAndSendArmiesToOccupy(defeatedPlayerName, attackingRegion, defeatedRegion);
            }
            else
            {
                ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion);
            }
        }

        private void GameIsOver()
        {
            _gamePhaseConductor.PlayerIsTheWinner(CurrentPlayerName);
        }

        private void AquireAllCardsFromPlayerAndSendArmiesToOccupy(PlayerName defeatedPlayerName, Region attackingRegion, Region defeatedRegion)
        {
            var defeatedPlayer = Players.Single(x => x.Name == defeatedPlayerName);
            var attackingPlayer = Players.Single(x => x.Name == CurrentPlayerName);

            defeatedPlayer.EliminatedBy(attackingPlayer);

            ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion);
        }

        private IPlayer GetCurrentPlayer()
        {
            return Players.Single(x => x.Name == CurrentPlayerName);
        }

        private void ContinueWithAttackOrOccupation(Region sourceRegion, Region destinationRegion)
        {
            var numberOfArmiesThatCanBeSentToOccupy = Territories
                .Single(x => x.Region == sourceRegion).GetNumberOfArmiesThatCanBeSentToOccupy();

            if (numberOfArmiesThatCanBeSentToOccupy > 0)
            {
                SendArmiesToOccupy(sourceRegion, destinationRegion);
            }
            else
            {
                MoveOnToAttackPhase();
            }
        }

        private void SendArmiesToOccupy(Region sourceRegion, Region destinationRegion)
        {
            _gamePhaseConductor.SendArmiesToOccupy(sourceRegion, destinationRegion);
        }

        private void MoveOnToAttackPhase()
        {
            _gamePhaseConductor.ContinueWithAttackPhase(_conqueringAchievement);
        }

        private bool CanAttack(Region attackingRegion, Region defendingRegion)
        {
            var attackingTerritory = Territories.Single(x => x.Region == attackingRegion);
            var defendingTerritory = Territories.Single(x => x.Region == defendingRegion);

            return IsCurrentPlayerOccupyingRegion(attackingRegion)
                   &&
                   _attackService.CanAttack(attackingTerritory, defendingTerritory);
        }

        private bool CanFortify(Region sourceRegion, Region destinationRegion)
        {
            var sourceTerritory = Territories.Single(x => x.Region == sourceRegion);
            var destinationTerritory = Territories.Single(x => x.Region == destinationRegion);
            var playerOccupiesBothTerritories = sourceTerritory.PlayerName == destinationTerritory.PlayerName;
            var hasBorder = IsTerritoriesAdjacent(sourceRegion, destinationRegion);
            var hasAtLeastOneArmyThatCanFortifyDestination = sourceTerritory.GetNumberOfArmiesThatCanFortifyAnotherTerritory() > 0;

            var canFortify =
                playerOccupiesBothTerritories
                &&
                hasBorder
                &&
                hasAtLeastOneArmyThatCanFortifyDestination;

            return canFortify;
        }

        private bool IsTerritoriesAdjacent(Region sourceRegion, Region destinationRegion)
        {
            return _worldMap.HasBorder(sourceRegion, destinationRegion);
        }

        private void MaybeAwardCard()
        {
            if (ShouldCardBeAwarded())
            {
                GetCurrentPlayer().AddCard(Deck);
            }
        }

        private bool ShouldCardBeAwarded()
        {
            return _conqueringAchievement == ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;
        }
    }
}