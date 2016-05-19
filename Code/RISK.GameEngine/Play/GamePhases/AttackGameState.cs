using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Core;
using RISK.GameEngine.Extensions;

namespace RISK.GameEngine.Play.GamePhases
{
    public class AttackGameState : IGameState
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IBattle _battle;
        private readonly IAttackPhaseRules _attackPhaseRules;
        private readonly GameData _gameData;
        private ConqueringAchievement _conqueringAchievement = ConqueringAchievement.DoNotAwardCardAtEndOfTurn;

        public AttackGameState(IGameStateConductor gameStateConductor, IGameDataFactory gameDataFactory, IBattle battle, IAttackPhaseRules attackPhaseRules, GameData gameData)
        {
            _gameStateConductor = gameStateConductor;
            _gameDataFactory = gameDataFactory;
            _battle = battle;
            _attackPhaseRules = attackPhaseRules;
            _gameData = gameData;
        }

        public IPlayer CurrentPlayer => _gameData.CurrentPlayer;
        public IReadOnlyList<IPlayer> Players => _gameData.Players;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IDeck Deck => _gameData.Deck;

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            var attackingTerritory = _gameData.Territories.GetTerritory(attackingRegion);

            if (!IsCurrentPlayerOccupyingTerritory(attackingTerritory))
            {
                return false;
            }

            return _attackPhaseRules.CanAttack(_gameData.Territories, attackingRegion, defendingRegion);
        }

        private bool IsCurrentPlayerOccupyingTerritory(ITerritory territory)
        {
            return _gameData.CurrentPlayer == territory.Player;
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            if (!CanAttack(attackingRegion, defendingRegion))
            {
                throw new InvalidOperationException();
            }

            var attackingTerritory = _gameData.Territories.GetTerritory(attackingRegion);
            var defendingTerritory = _gameData.Territories.GetTerritory(defendingRegion);
            var defendingPlayer = _gameData.Players.Single(player => player == defendingTerritory.Player);

            var battleResult = _battle.Attack(attackingTerritory, defendingTerritory);

            var updatedTerritories = _gameData.Territories
                .Replace(attackingTerritory, battleResult.UpdatedAttackingTerritory)
                .Replace(defendingTerritory, battleResult.UpdatedDefendingTerritory)
                .ToList();

            if (battleResult.IsDefenderDefeated())
            {
                DefenderIsDefeated(defendingPlayer, attackingRegion, defendingRegion, updatedTerritories);
            }
            else
            {
                MoveOnToAttackPhase(updatedTerritories);
            }
        }

        private void DefenderIsDefeated(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _conqueringAchievement = ConqueringAchievement.AwardCardAtEndOfTurn;

            if (_attackPhaseRules.IsGameOver(updatedTerritories))
            {
                GameIsOver(updatedTerritories);
            }
            else
            {
                var isPlayerEliminated = _attackPhaseRules.IsPlayerEliminated(updatedTerritories, defeatedPlayer);
                if (isPlayerEliminated)
                {
                    AquireAllCardsFromEliminatedPlayer(defeatedPlayer);
                }

                SendArmiesToOccupy(updatedTerritories, attackingRegion, defeatedRegion);
            }
        }

        private void GameIsOver(IReadOnlyList<ITerritory> territories)
        {
            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, Players, territories, Deck);

            _gameStateConductor.GameIsOver(gameData);
        }

        private void AquireAllCardsFromEliminatedPlayer(IPlayer eliminatedPlayer)
        {
            var aquiredCards = eliminatedPlayer.AquireAllCards();
            foreach (var aquiredCard in aquiredCards)
            {
                _gameData.CurrentPlayer.AddCard(aquiredCard);
            }
        }

        private void SendArmiesToOccupy(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion)
        {
            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, Players, territories, Deck);

            _gameStateConductor.SendArmiesToOccupy(gameData, sourceRegion, destinationRegion);
        }

        private void MoveOnToAttackPhase(IReadOnlyList<ITerritory> updatedTerritories)
        {
            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, Players, updatedTerritories, Deck);

            _gameStateConductor.ContinueWithAttackPhase(gameData, _conqueringAchievement);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            var sourceTerritory = _gameData.Territories.GetTerritory(sourceRegion);

            if (!IsCurrentPlayerOccupyingTerritory(sourceTerritory))
            {
                return false;
            }

            return _attackPhaseRules.CanFortify(_gameData.Territories, sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            if (!CanFortify(sourceRegion, destinationRegion))
            {
                throw new InvalidOperationException();
            }

            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, Players, _gameData.Territories, Deck);

            _gameStateConductor.Fortify(gameData, sourceRegion, destinationRegion, armies);
        }

        public bool CanEndTurn()
        {
            return true;
        }

        public void EndTurn()
        {
            if (_conqueringAchievement == ConqueringAchievement.AwardCardAtEndOfTurn)
            {
                var card = Deck.Draw();
                _gameData.CurrentPlayer.AddCard(card);
            }

            _gameStateConductor.PassTurnToNextPlayer(this);
        }

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return false;
        }

        public int GetNumberOfArmiesToDraft()
        {
            throw new InvalidOperationException();
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            throw new InvalidOperationException();
        }

        public bool CanSendAdditionalArmiesToOccupy()
        {
            return false;
        }

        public int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy()
        {
            throw new InvalidOperationException();
        }

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            throw new InvalidOperationException();
        }
    }
}