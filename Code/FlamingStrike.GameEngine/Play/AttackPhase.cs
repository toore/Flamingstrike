using System;
using System.Collections.Generic;
using System.Linq;
using FlamingStrike.Core;

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
            IReadOnlyList<IPlayerGameData> playerGameDatas,
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
            PlayerGameDatas = playerGameDatas;
            Deck = deck;
        }

        public PlayerName CurrentPlayerName { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
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
                MoveOnToAttackPhase(attackOutcome.Territories, PlayerGameDatas);
            }
        }

        public void Fortify(Region sourceRegion, Region destinationRegion, int armies)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(sourceRegion)}.");
            }

            var updatedTerritories = _fortifier.Fortify(Territories, sourceRegion, destinationRegion, armies);
            var playersAndDeck = UpdatePlayerGameDatasAndDeck();
            var updatedGameData = new GameData(updatedTerritories, playersAndDeck.PlayerGameDatas, CurrentPlayerName, playersAndDeck.Deck);

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
            var playersAndDeck = UpdatePlayerGameDatasAndDeck();

            var updatedGameData = new GameData(Territories, playersAndDeck.PlayerGameDatas, CurrentPlayerName, playersAndDeck.Deck);

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
                ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion, updatedTerritories, PlayerGameDatas);
            }
        }

        private void GameIsOver()
        {
            _gamePhaseConductor.PlayerIsTheWinner(CurrentPlayerName);
        }

        private void AquireAllCardsFromPlayerAndSendArmiesToOccupy(PlayerName defeatedPlayerName, Region attackingRegion, Region defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            var eliminatedPlayerGameData = PlayerGameDatas.Single(x => x.PlayerName == defeatedPlayerName);

            var updatedCurrentPlayer = LetCurrentPlayerAquireAllCardsFromEliminatedPlayer(eliminatedPlayerGameData);
            var updatedEliminatedPlayer = RemoveAllCardsForPlayer(eliminatedPlayerGameData);

            var updatedPlayerGameDatas = PlayerGameDatas
                .Replace(GetCurrentPlayerGameData(), updatedCurrentPlayer)
                .Replace(eliminatedPlayerGameData, updatedEliminatedPlayer)
                .ToList();

            ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion, updatedTerritories, updatedPlayerGameDatas);
        }

        private IPlayerGameData GetCurrentPlayerGameData()
        {
            return PlayerGameDatas.Single(x => x.PlayerName == CurrentPlayerName);
        }

        private PlayerGameData LetCurrentPlayerAquireAllCardsFromEliminatedPlayer(IPlayerGameData eliminatedPlayerGameData)
        {
            return new PlayerGameData(
                CurrentPlayerName,
                GetCurrentPlayerGameData().Cards.Concat(eliminatedPlayerGameData.Cards).ToList());
        }

        private static PlayerGameData RemoveAllCardsForPlayer(IPlayerGameData eliminatedPlayerGameData)
        {
            return new PlayerGameData(eliminatedPlayerGameData.PlayerName, new List<ICard>());
        }

        private void ContinueWithAttackOrOccupation(Region sourceRegion, Region destinationRegion, IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayerGameData> updatedPlayerGameDatas)
        {
            var numberOfArmiesThatCanBeSentToOccupy = updatedTerritories
                .Single(x => x.Region == sourceRegion).GetNumberOfArmiesThatCanBeSentToOccupy();

            if (numberOfArmiesThatCanBeSentToOccupy > 0)
            {
                SendArmiesToOccupy(sourceRegion, destinationRegion, updatedTerritories, updatedPlayerGameDatas);
            }
            else
            {
                MoveOnToAttackPhase(updatedTerritories, updatedPlayerGameDatas);
            }
        }

        private void SendArmiesToOccupy(Region sourceRegion, Region destinationRegion, IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayerGameData> updatedPlayerGameDatas)
        {
            var updatedGameData = new GameData(updatedTerritories, updatedPlayerGameDatas, CurrentPlayerName, Deck);

            _gamePhaseConductor.SendArmiesToOccupy(sourceRegion, destinationRegion, updatedGameData);
        }

        private void MoveOnToAttackPhase(IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayerGameData> updatedPlayerGameDatas)
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

        private PlayerGameDatasAndDeck UpdatePlayerGameDatasAndDeck()
        {
            var updatedPlayerGameDatas = AwardCard()
                .Bind(x => AddCardToPlayer(GetCurrentPlayerGameData(), x.TopCard))
                .Fold(
                    x => PlayerGameDatas.Replace(GetCurrentPlayerGameData(), x).ToList(),
                    () => PlayerGameDatas);

            var updatedDeck = AwardCard()
                .Fold(x => x.RestOfTheDeck, () => Deck);

            return new PlayerGameDatasAndDeck(updatedPlayerGameDatas, updatedDeck);
        }

        private Maybe<DrawCard> AwardCard()
        {
            if (ShouldCardBeAwarded())
            {
                return Maybe<DrawCard>.Create(Deck.DrawCard());
            }

            return Maybe<DrawCard>.Nothing;
        }

        private bool ShouldCardBeAwarded()
        {
            return _conqueringAchievement == ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;
        }

        private PlayerGameData AddCardToPlayer(IPlayerGameData playerGameData, ICard card)
        {
            var playerCards = playerGameData.Cards.Concat(new[] { card }).ToList();

            return new PlayerGameData(CurrentPlayerName, playerCards);
        }

        private class PlayerGameDatasAndDeck
        {
            public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
            public IDeck Deck { get; }

            public PlayerGameDatasAndDeck(IReadOnlyList<IPlayerGameData> playerGameDatas, IDeck deck)
            {
                PlayerGameDatas = playerGameDatas;
                Deck = deck;
            }
        }
    }
}