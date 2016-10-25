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
            var updatedGameData = new GameData(attackOutcome.Territories, _gameData.PlayerGameDatas, _gameData.CurrentPlayer, _gameData.Deck);

            if (attackOutcome.DefendingArmyAvailability == DefendingArmyAvailability.IsEliminated)
            {
                var defeatedPlayer = defendingPlayerBeforeTerritoriesAreUpdated;
                DefendingArmyIsEliminated(defeatedPlayer, attackingRegion, defendingRegion, updatedGameData);
            }
            else
            {
                MoveOnToAttackPhase(updatedGameData);
            }
        }

        private IPlayer GetPlayer(IRegion region)
        {
            return _gameData.Territories.Single(x => x.Region == region).Player;
        }

        private void DefendingArmyIsEliminated(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, GameData gameData)
        {
            _turnConqueringAchievement = TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            if (_playerEliminationRules.IsOnlyOnePlayerLeftInTheGame(gameData.Territories))
            {
                GameIsOver();
            }
            else if (_playerEliminationRules.IsPlayerEliminated(gameData.Territories, defeatedPlayer))
            {
                AquireAllCardsFromPlayerAndSendArmiesToOccupy(defeatedPlayer, attackingRegion, defeatedRegion, gameData);
            }
            else
            {
                ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion, gameData);
            }
        }

        private void GameIsOver()
        {
            _gamePhaseConductor.PlayerIsTheWinner(_gameData.CurrentPlayer);
        }

        private void AquireAllCardsFromPlayerAndSendArmiesToOccupy(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, GameData gameData)
        {
            var eliminatedPlayer = _gameData.PlayerGameDatas.Single(x => x.Player == defeatedPlayer);

            AquireAllCardsFromEliminatedPlayer(eliminatedPlayer);
            ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion, gameData);
        }

        private void AquireAllCardsFromEliminatedPlayer(IPlayerGameData eliminatedPlayerGameData)
        {
            throw new NotImplementedException();
            //var aquiredCards = eliminatedPlayerGameData.AquireAllCards();
            //foreach (var aquiredCard in aquiredCards)
            //{
            //    _currentPlayerGameData.AddCard(aquiredCard);
            //}
        }

        private void ContinueWithAttackOrOccupation(IRegion sourceRegion, IRegion destinationRegion, GameData gameData)
        {
            var numberOfArmiesThatCanBeSentToOccupy = gameData.Territories
                .Single(x => x.Region == sourceRegion).GetNumberOfArmiesThatCanBeSentToOccupy();

            if (numberOfArmiesThatCanBeSentToOccupy > 0)
            {
                SendArmiesToOccupy(sourceRegion, destinationRegion, gameData);
            }
            else
            {
                MoveOnToAttackPhase(gameData);
            }
        }

        private void SendArmiesToOccupy(IRegion sourceRegion, IRegion destinationRegion, GameData gameData)
        {
            _gamePhaseConductor.SendArmiesToOccupy(sourceRegion, destinationRegion, gameData);
        }

        private void MoveOnToAttackPhase(GameData gameData)
        {
            _gamePhaseConductor.ContinueWithAttackPhase(_turnConqueringAchievement, gameData);
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
            var updatedPlayerGameDatas = MaybeDrawCardAndUpdatePlayerGameDatas();
            var updatedDeck = MaybeDrawCard()
                .Fold(x => x.RestOfDeck, () => _gameData.Deck);
            var updatedGameData = new GameData(updatedTerritories, updatedPlayerGameDatas, _gameData.CurrentPlayer, updatedDeck);

            _gamePhaseConductor.WaitForTurnToEnd(updatedGameData);
        }

        public void EndTurn()
        {
            var updatedPlayerGameDatas = MaybeDrawCardAndUpdatePlayerGameDatas();
            var updatedDeck = MaybeDrawCard()
                .Fold(x => x.RestOfDeck, () => _gameData.Deck);

            var updatedGameData = new GameData(_gameData.Territories, updatedPlayerGameDatas, _gameData.CurrentPlayer, updatedDeck);

            _gamePhaseConductor.PassTurnToNextPlayer(updatedGameData);
        }

        private List<IPlayerGameData> MaybeDrawCardAndUpdatePlayerGameDatas()
        {
            var currentPlayerCards = _gameData.GetCurrentPlayerGameData().Cards;
            var updatedPlayerCards = MaybeDrawCard()
                .Fold(x => currentPlayerCards.Concat(new[] { x.CardDrawn }).ToList(),
                    () => _gameData.GetCurrentPlayerGameData().Cards);

            var updatedCurrentPlayer = new PlayerGameData(_gameData.CurrentPlayer, updatedPlayerCards);

            return _gameData.PlayerGameDatas.Replace(_gameData.GetCurrentPlayerGameData(), updatedCurrentPlayer).ToList();
        }

        private Maybe<CardDrawnAndRestOfDeck> MaybeDrawCard()
        {
            if (ShouldCardBeAwarded())
            {
                return Maybe<CardDrawnAndRestOfDeck>.Create(_gameData.Deck.DrawCard());
            }

            return Maybe<CardDrawnAndRestOfDeck>.Nothing;
        }

        private bool ShouldCardBeAwarded()
        {
            return _turnConqueringAchievement == TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;
        }
    }
}