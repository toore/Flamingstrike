using System;
using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine.Attacking;

namespace RISK.GameEngine.Play.GameStates
{
    public interface IAttackPhaseGameState
    {
        IPlayer Player { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> Players { get; }
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        void EndTurn();
    }

    public class AttackPhaseStateGameState : IAttackPhaseGameState
    {
        private readonly GameData _gameData;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private TurnConqueringAchievement _turnConqueringAchievement;

        public AttackPhaseStateGameState(
            GameData gameData,
            IGamePhaseConductor gamePhaseConductor,
            IAttacker attacker,
            IFortifier fortifier,
            IPlayerEliminationRules playerEliminationRules,
            TurnConqueringAchievement turnConqueringAchievement)
        {
            _gameData = gameData;
            _gamePhaseConductor = gamePhaseConductor;
            _attacker = attacker;
            _fortifier = fortifier;
            _playerEliminationRules = playerEliminationRules;
            _turnConqueringAchievement = turnConqueringAchievement;
        }

        public IPlayer Player => _gameData.CurrentPlayer;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IReadOnlyList<IPlayerGameData> Players => _gameData.PlayerGameDatas;

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return IsCurrentPlayerOccupyingRegion(attackingRegion)
                &&
                _attacker.CanAttack(_gameData.Territories, attackingRegion, defendingRegion);
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return _gameData.Territories
                .Single(x => x.Region == region)
                .Player == _gameData.CurrentPlayer;
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(attackingRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(attackingRegion)}.");
            }

            var defendingPlayerBeforeTerritoriesAreUpdated = GetPlayer(defendingRegion);

            var attackOutcome = _attacker.Attack(_gameData.Territories, attackingRegion, defendingRegion);

            if (attackOutcome.DefendingArmyAvailability == DefendingArmyAvailability.IsEliminated)
            {
                var defeatedPlayer = defendingPlayerBeforeTerritoriesAreUpdated;
                DefendingArmyIsEliminated(defeatedPlayer, attackingRegion, defendingRegion, attackOutcome.Territories);
            }
            else
            {
                MoveOnToAttackPhase(attackOutcome.Territories, _gameData.PlayerGameDatas);
            }
        }

        private IPlayer GetPlayer(IRegion region)
        {
            return _gameData.Territories.Single(x => x.Region == region).Player;
        }

        private void DefendingArmyIsEliminated(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _turnConqueringAchievement = TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            if (_playerEliminationRules.IsOnlyOnePlayerLeftInTheGame(updatedTerritories))
            {
                GameIsOver();
            }
            else if (_playerEliminationRules.IsPlayerEliminated(updatedTerritories, defeatedPlayer))
            {
                AquireAllCardsFromPlayerAndSendArmiesToOccupy(defeatedPlayer, attackingRegion, defeatedRegion, updatedTerritories);
            }
            else
            {
                ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion, updatedTerritories, _gameData.PlayerGameDatas);
            }
        }

        private void GameIsOver()
        {
            _gamePhaseConductor.PlayerIsTheWinner(_gameData.CurrentPlayer);
        }

        private void AquireAllCardsFromPlayerAndSendArmiesToOccupy(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            var eliminatedPlayerGameData = _gameData.PlayerGameDatas.Single(x => x.Player == defeatedPlayer);

            var updatedCurrentPlayer = LetCurrentPlayerAquireAllCardsFromEliminatedPlayer(eliminatedPlayerGameData);
            var updatedEliminatedPlayer = RemoveAllCardsForPlayer(eliminatedPlayerGameData);

            var updatedPlayerGameDatas = _gameData.PlayerGameDatas
                .Replace(_gameData.GetCurrentPlayerGameData(), updatedCurrentPlayer)
                .Replace(eliminatedPlayerGameData, updatedEliminatedPlayer)
                .ToList();

            ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion, updatedTerritories, updatedPlayerGameDatas);
        }

        private PlayerGameData LetCurrentPlayerAquireAllCardsFromEliminatedPlayer(IPlayerGameData eliminatedPlayerGameData)
        {
            return new PlayerGameData(
                _gameData.CurrentPlayer,
                _gameData.GetCurrentPlayerGameData().Cards.Concat(eliminatedPlayerGameData.Cards).ToList());
        }

        private static PlayerGameData RemoveAllCardsForPlayer(IPlayerGameData eliminatedPlayerGameData)
        {
            return new PlayerGameData(eliminatedPlayerGameData.Player, new List<ICard>());
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
            var updatedGameData = new GameData(updatedTerritories, updatedPlayerGameDatas, _gameData.CurrentPlayer, _gameData.Deck);

            _gamePhaseConductor.SendArmiesToOccupy(sourceRegion, destinationRegion, updatedGameData);
        }

        private void MoveOnToAttackPhase(IReadOnlyList<ITerritory> updatedTerritories, IReadOnlyList<IPlayerGameData> updatedPlayerGameDatas)
        {
            var updatedGameData = new GameData(updatedTerritories, updatedPlayerGameDatas, _gameData.CurrentPlayer, _gameData.Deck);

            _gamePhaseConductor.ContinueWithAttackPhase(_turnConqueringAchievement, updatedGameData);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                return false;
            }

            return _fortifier.CanFortify(_gameData.Territories, sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(sourceRegion)}.");
            }

            var updatedTerritories = _fortifier.Fortify(_gameData.Territories, sourceRegion, destinationRegion, armies);
            var playersAndDeck = UpdatePlayerGameDatasAndDeck();
            var updatedGameData = new GameData(updatedTerritories, playersAndDeck.PlayerGameDatas, _gameData.CurrentPlayer, playersAndDeck.Deck);

            _gamePhaseConductor.WaitForTurnToEnd(updatedGameData);
        }

        public void EndTurn()
        {
            var playersAndDeck = UpdatePlayerGameDatasAndDeck();

            var updatedGameData = new GameData(_gameData.Territories, playersAndDeck.PlayerGameDatas, _gameData.CurrentPlayer, playersAndDeck.Deck);

            _gamePhaseConductor.PassTurnToNextPlayer(updatedGameData);
        }

        private PlayerGameDatasAndDeck UpdatePlayerGameDatasAndDeck()
        {
            var updatedPlayerGameDatas = AwardCard()
                .Bind(x => AddCardToPlayer(_gameData.GetCurrentPlayerGameData(), x.TopCard))
                .Fold(x => _gameData.PlayerGameDatas.Replace(_gameData.GetCurrentPlayerGameData(), x).ToList(),
                    () => _gameData.PlayerGameDatas);

            var updatedDeck = AwardCard()
                .Fold(x => x.RestOfTheDeck, () => _gameData.Deck);

            return new PlayerGameDatasAndDeck(updatedPlayerGameDatas, updatedDeck);
        }

        private Maybe<DrawCard> AwardCard()
        {
            if (ShouldCardBeAwarded())
            {
                return Maybe<DrawCard>.Create(_gameData.Deck.DrawCard());
            }

            return Maybe<DrawCard>.Nothing;
        }

        private bool ShouldCardBeAwarded()
        {
            return _turnConqueringAchievement == TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;
        }

        private PlayerGameData AddCardToPlayer(IPlayerGameData playerGameData, ICard card)
        {
            var playerCards = playerGameData.Cards.Concat(new[] { card }).ToList();

            return new PlayerGameData(_gameData.CurrentPlayer, playerCards);
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