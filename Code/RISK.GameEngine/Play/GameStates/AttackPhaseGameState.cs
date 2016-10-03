using System;
using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine.Attacking;

namespace RISK.GameEngine.Play.GameStates
{
    public interface IAttackPhaseGameState
    {
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        void EndTurn();
    }

    public class AttackPhaseStateGameState : IAttackPhaseGameState
    {
        private readonly IPlayer _currentPlayer;
        private readonly IReadOnlyList<IPlayer> _players;
        private readonly ITerritoriesContext _territoriesContext;
        private readonly IDeck _deck;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private TurnConqueringAchievement _turnConqueringAchievement;
        private bool _cardHasBeenAwardedThisTurn;

        public AttackPhaseStateGameState(
            IPlayer currentPlayer,
            IReadOnlyList<IPlayer> players,
            ITerritoriesContext territoriesContext,
            IDeck deck,
            IGamePhaseConductor gamePhaseConductor,
            IAttacker attacker,
            IFortifier fortifier,
            IPlayerEliminationRules playerEliminationRules,
            TurnConqueringAchievement turnConqueringAchievement)
        {
            _currentPlayer = currentPlayer;
            _players = players;
            _territoriesContext = territoriesContext;
            _deck = deck;
            _gamePhaseConductor = gamePhaseConductor;
            _attacker = attacker;
            _fortifier = fortifier;
            _playerEliminationRules = playerEliminationRules;
            _turnConqueringAchievement = turnConqueringAchievement;
        }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return IsCurrentPlayerOccupyingRegion(attackingRegion)
                &&
                _attacker.CanAttack(_territoriesContext.Territories, attackingRegion, defendingRegion);
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return _territoriesContext.Territories
                .Single(x => x.Region == region)
                .Player == _currentPlayer;
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(attackingRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(attackingRegion)}.");
            }

            var defendingPlayerBeforeTerritoriesAreUpdated = GetPlayer(defendingRegion);

            var attackOutcome = _attacker.Attack(_territoriesContext.Territories, attackingRegion, defendingRegion);
            _territoriesContext.Set(attackOutcome.Territories);

            if (attackOutcome.DefendingArmyAvailability == DefendingArmyAvailability.IsEliminated)
            {
                var defeatedPlayer = defendingPlayerBeforeTerritoriesAreUpdated;
                DefendingArmyIsEliminated(defeatedPlayer, attackingRegion, defendingRegion);
            }
            else
            {
                MoveOnToAttackPhase();
            }
        }

        private IPlayer GetPlayer(IRegion region)
        {
            return _territoriesContext.Territories.Single(x => x.Region == region).Player;
        }

        private void DefendingArmyIsEliminated(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion)
        {
            _turnConqueringAchievement = TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            if (_playerEliminationRules.IsOnlyOnePlayerLeftInTheGame(_territoriesContext.Territories))
            {
                GameIsOver();
            }
            else if (_playerEliminationRules.IsPlayerEliminated(_territoriesContext.Territories, defeatedPlayer))
            {
                AquireAllCardsFromPlayerAndSendArmiesToOccupy(defeatedPlayer, attackingRegion, defeatedRegion);
            }
            else
            {
                ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion);
            }
        }

        private void GameIsOver()
        {
            _gamePhaseConductor.PlayerIsTheWinner(_currentPlayer);
        }

        private void AquireAllCardsFromPlayerAndSendArmiesToOccupy(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion)
        {
            var eliminatedPlayer = _players.Single(player => player == defeatedPlayer);

            AquireAllCardsFromEliminatedPlayer(eliminatedPlayer);
            ContinueWithAttackOrOccupation(attackingRegion, defeatedRegion);
        }

        private void AquireAllCardsFromEliminatedPlayer(IPlayer eliminatedPlayer)
        {
            var aquiredCards = eliminatedPlayer.AquireAllCards();
            foreach (var aquiredCard in aquiredCards)
            {
                _currentPlayer.AddCard(aquiredCard);
            }
        }

        private void ContinueWithAttackOrOccupation(IRegion sourceRegion, IRegion destinationRegion)
        {
            var numberOfArmiesThatCanBeSentToOccupy = _territoriesContext.Territories
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

        private void SendArmiesToOccupy(IRegion sourceRegion, IRegion destinationRegion)
        {
            _gamePhaseConductor.SendArmiesToOccupy(sourceRegion, destinationRegion);
        }

        private void MoveOnToAttackPhase()
        {
            _gamePhaseConductor.ContinueWithAttackPhase(_turnConqueringAchievement);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                return false;
            }

            return _fortifier.CanFortify(_territoriesContext.Territories, sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(sourceRegion)}.");
            }

            MaybeCardIsAwarded();

            var updatedTerritories = _fortifier.Fortify(_territoriesContext.Territories, sourceRegion, destinationRegion, armies);
            _territoriesContext.Set(updatedTerritories);

            _gamePhaseConductor.WaitForTurnToEnd();
        }

        public void EndTurn()
        {
            MaybeCardIsAwarded();

            _gamePhaseConductor.PassTurnToNextPlayer();
        }

        private void MaybeCardIsAwarded()
        {
            if (ShouldCardBeAwarded())
            {
                AddTopDeckCardToCurrentPlayersCards();
                _cardHasBeenAwardedThisTurn = true;
            }
        }

        private bool ShouldCardBeAwarded()
        {
            return _turnConqueringAchievement == TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory
                &&
                !_cardHasBeenAwardedThisTurn;
        }

        private void AddTopDeckCardToCurrentPlayersCards()
        {
            var card = _deck.Draw();
            _currentPlayer.AddCard(card);
        }
    }
}