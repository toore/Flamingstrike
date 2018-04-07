using System;
using System.Collections.Generic;
using System.Linq;
using FlamingStrike.Core;

namespace FlamingStrike.GameEngine.Play
{
    public interface IAttackPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        IReadOnlyList<IRegion> GetRegionsThatCanBeSourceForAttackOrFortification();
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        IEnumerable<IRegion> GetRegionsThatCanBeAttacked(IRegion sourceRegion);
        IEnumerable<IRegion> GetRegionsThatCanBeFortified(IRegion sourceRegion);
        void EndTurn();
    }

    public class AttackPhase : IAttackPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;
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
            IPlayerEliminationRules playerEliminationRules)
        {
            _gamePhaseConductor = gamePhaseConductor;
            _conqueringAchievement = conqueringAchievement;
            _attacker = attacker;
            _fortifier = fortifier;
            _playerEliminationRules = playerEliminationRules;
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
            Deck = deck;
        }

        public PlayerName CurrentPlayerName { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        public IDeck Deck { get; }

        public IReadOnlyList<IRegion> GetRegionsThatCanBeSourceForAttackOrFortification()
        {
            return Territories
                .Where(x => IsCurrentPlayerOccupyingRegion(x.Region))
                .Select(x => x.Region)
                .ToList();
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
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

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
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

        public IEnumerable<IRegion> GetRegionsThatCanBeAttacked(IRegion sourceRegion)
        {
            return sourceRegion.GetBorderingRegions()
                .Where(borderRegion => CanAttack(sourceRegion, borderRegion));
        }

        public IEnumerable<IRegion> GetRegionsThatCanBeFortified(IRegion sourceRegion)
        {
            return sourceRegion.GetBorderingRegions()
                .Where(borderRegion => CanFortify(sourceRegion, borderRegion));
        }

        public void EndTurn()
        {
            var playersAndDeck = UpdatePlayerGameDatasAndDeck();

            var updatedGameData = new GameData(Territories, playersAndDeck.PlayerGameDatas, CurrentPlayerName, playersAndDeck.Deck);

            _gamePhaseConductor.PassTurnToNextPlayer(updatedGameData);
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return Territories.Single(x => x.Region == region).PlayerName == CurrentPlayerName;
        }

        private PlayerName GetPlayer(IRegion region)
        {
            return Territories.Single(x => x.Region == region).PlayerName;
        }

        private void DefendingArmyIsEliminated(PlayerName defeatedPlayerName, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _conqueringAchievement = ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            if (_playerEliminationRules.IsOnlyOnePlayerLeftInTheGame(updatedTerritories))
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

        private void AquireAllCardsFromPlayerAndSendArmiesToOccupy(PlayerName defeatedPlayerName, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
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

        private void ContinueWithAttackOrOccupation(IRegion sourceRegion, IRegion destinationRegion, IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayerGameData> updatedPlayerGameDatas)
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

        private void SendArmiesToOccupy(IRegion sourceRegion, IRegion destinationRegion, IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayerGameData> updatedPlayerGameDatas)
        {
            var updatedGameData = new GameData(updatedTerritories, updatedPlayerGameDatas, CurrentPlayerName, Deck);

            _gamePhaseConductor.SendArmiesToOccupy(sourceRegion, destinationRegion, updatedGameData);
        }

        private void MoveOnToAttackPhase(IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayerGameData> updatedPlayerGameDatas)
        {
            var updatedGameData = new GameData(updatedTerritories, updatedPlayerGameDatas, CurrentPlayerName, Deck);

            _gamePhaseConductor.ContinueWithAttackPhase(_conqueringAchievement, updatedGameData);
        }

        private bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return IsCurrentPlayerOccupyingRegion(attackingRegion)
                   &&
                   _attacker.CanAttack(Territories, attackingRegion, defendingRegion);
        }

        private bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
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