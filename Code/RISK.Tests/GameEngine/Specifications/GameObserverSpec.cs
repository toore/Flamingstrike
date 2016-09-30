using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GameStates;
using RISK.GameEngine.Setup;
using Toore.Shuffling;
using Xunit;
using IPlayer = RISK.GameEngine.IPlayer;

namespace RISK.Tests.GameEngine.Specifications
{
    public class GameObserverSpec : SpecBase<GameObserverSpec>
    {
        private Regions _regions;
        private IPlayer _player1;
        private IPlayer _player2;
        private IDice _dice;
        private IReadOnlyList<IPlayer> _players;
        private readonly List<Territory> _territories = new List<Territory>();
        private Continents _continents;
        private readonly GameObserverSpy _gameObserverSpy = new GameObserverSpy();

        [Fact]
        public void First_players_draft_armies()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started();

            When
                .player_drafts_armies_in_north_africa();

            Then
                .player_1_should_occupy_north_africa_with_six_armies();
        }

        [Fact]
        public void Second_player_draft_armies()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_thirtyfive_armies_in_scandinavia()
                .player_ends_turn();

            When
                .player_drafts_three_armies_in_brazil();

            Then
                .player_2_should_occupy_brazil_with_4_armies();
        }

        [Fact]
        public void First_player_draft_armies_second_turn()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_thirtyfive_armies_in_scandinavia()
                .player_ends_turn()
                .player_drafts_three_armies_in_brazil();

            When
                .player_ends_turn();

            Then
                .player_1_should_take_turn();
        }

        [Fact]
        public void First_player_occupies_brazil_after_win()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_three_armies_in_north_africa()
                .player_drafts_thirtytwo_armies_in_iceland();

            When
                .player_attacks_brazil_from_north_africa_and_wins()
                .two_additional_armies_is_sent_to_occupy();

            Then
                .player_1_should_occupy_brazil_with_five_armies()
                .player_1_should_occupy_north_africa_with_three_armies();
        }

        [Fact]
        public void Receives_a_card_when_ending_turn()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_three_armies_in_north_africa()
                .player_drafts_thirtytwo_armies_in_iceland();

            When
                .player_attacks_brazil_from_north_africa_and_wins()
                .two_additional_armies_is_sent_to_occupy()
                .player_ends_turn();

            Then
                .player_1_should_have_a_card();
        }

        [Fact]
        public void Does_not_receive_a_card_when_ending_turn()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_three_armies_in_north_africa()
                .player_drafts_thirtytwo_armies_in_iceland();

            When
                .player_ends_turn();

            Then
                .player_1_should_not_have_any_card();
        }

        [Fact]
        public void Fortifies_armies()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_three_armies_in_north_africa()
                .player_drafts_thirtytwo_armies_in_iceland();

            When
                .player_moves_one_army_from_north_africa_to_east_africa();

            Then
                .east_africa_should_have_two_armies();
        }

        [Fact]
        public void Game_over_when_player_occupies_all_territories()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_with_one_army_each()
                .player_2_occupies_brazil_with_one_army()
                .game_is_started()
                .player_drafts_thirtyfive_armies_in_north_africa();

            When.
                player_attacks_brazil_from_north_africa_and_wins();

            Then.
                player_1_is_the_winner();
        }

        private void player_drafts_armies_in_north_africa()
        {
            _gameObserverSpy.DraftArmiesPhase.PlaceDraftArmies(_regions.NorthAfrica, 1);
        }

        private void player_1_should_have_a_card()
        {
            _player1.Cards.Count().Should().Be(1);
        }

        private void player_1_should_not_have_any_card()
        {
            _player1.Cards.Should().BeEmpty();
        }

        private void player_moves_one_army_from_north_africa_to_east_africa()
        {
            _gameObserverSpy.AttackPhase.Fortify(_regions.NorthAfrica, _regions.EastAfrica, 1);
        }

        private GameObserverSpec a_game_with_two_human_players()
        {
            _continents = new Continents();
            _regions = new Regions(_continents);

            _player1 = new Player("Player 1");
            _player2 = new Player("Player 2");

            return this;
        }

        private GameObserverSpec game_is_started()
        {
            _dice = Substitute.For<IDice>();
            var dicesRoller = new DicesRoller(_dice);
            var armyDraftCalculator = new ArmyDraftCalculator(_continents);
            var battle = new Battle(dicesRoller, new ArmiesLostCalculator());
            var armyDrafter = new ArmyDrafter();
            var territoryOccupier = new TerritoryOccupier();
            var fortifier = new Fortifier();
            var attacker = new Attacker(battle);
            var gameRules = new GameRules();
            var gameStateFactory = new GameStateFactory(gameRules, armyDrafter, attacker, territoryOccupier, fortifier);
            var fisherYatesShuffle = new FisherYatesShuffle(new RandomWrapper());
            var deckFactory = new DeckFactory(_regions, fisherYatesShuffle);
            var gameFactory = new GameFactory(gameStateFactory, armyDraftCalculator, deckFactory);
            _players = new List<IPlayer> { _player1, _player2 };
            var gamePlaySetup = new GamePlaySetup(_players, _territories);
            var game = gameFactory.Create(_gameObserverSpy, gamePlaySetup);

            return this;
        }

        private GameObserverSpec player_drafts_three_armies_in_north_africa()
        {
            _gameObserverSpy.DraftArmiesPhase.PlaceDraftArmies(_regions.NorthAfrica, 3);
            return this;
        }

        private void player_drafts_thirtytwo_armies_in_iceland()
        {
            _gameObserverSpy.DraftArmiesPhase.PlaceDraftArmies(_regions.Iceland, 32);
        }

        private GameObserverSpec player_drafts_thirtyfive_armies_in_scandinavia()
        {
            _gameObserverSpy.DraftArmiesPhase.PlaceDraftArmies(_regions.Scandinavia, 35);
            return this;
        }

        private void player_drafts_thirtyfive_armies_in_north_africa()
        {
            _gameObserverSpy.DraftArmiesPhase.PlaceDraftArmies(_regions.NorthAfrica, 35);
        }

        private void player_drafts_three_armies_in_brazil()
        {
            _gameObserverSpy.DraftArmiesPhase.PlaceDraftArmies(_regions.Brazil, 3);
        }

        private GameObserverSpec player_1_occupies_north_africa_with_five_armies()
        {
            AddTerritoryToGame(_regions.NorthAfrica, _player1, 5);
            return this;
        }

        private void AddTerritoryToGame(IRegion region, IPlayer player, int armies)
        {
            _territories.Add(new Territory(region, player, armies));
        }

        private GameObserverSpec player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
        {
            GetAllRegionsExcept(
                    _regions.Brazil,
                    _regions.Venezuela,
                    _regions.NorthAfrica).
                Apply(x => AddTerritoryToGame(x, _player1, 1));

            return this;
        }

        private GameObserverSpec player_2_occupies_brazil_and_venezuela_with_one_army_each()
        {
            AddTerritoryToGame(_regions.Brazil, _player2, 1);
            AddTerritoryToGame(_regions.Venezuela, _player2, 1);
            return this;
        }

        private GameObserverSpec player_1_occupies_every_territory_except_brazil_with_one_army_each()
        {
            GetAllRegionsExcept(
                    _regions.Brazil).
                Apply(x => AddTerritoryToGame(x, _player1, 1));
            return this;
        }

        private GameObserverSpec player_2_occupies_brazil_with_one_army()
        {
            AddTerritoryToGame(_regions.Brazil, _player2, 1);
            return this;
        }

        private IEnumerable<IRegion> GetAllRegionsExcept(params IRegion[] exceptRegions)
        {
            return _regions.GetAll().Except(exceptRegions);
        }

        private GameObserverSpec player_attacks_brazil_from_north_africa_and_wins()
        {
            _dice.Roll().Returns(6, 6, 6, 1);
            _gameObserverSpy.AttackPhase.Attack(_regions.NorthAfrica, _regions.Brazil);
            return this;
        }

        private GameObserverSpec two_additional_armies_is_sent_to_occupy()
        {
            _gameObserverSpy.SendArmiesToOccupyPhase.SendAdditionalArmiesToOccupy(2);
            return this;
        }

        private GameObserverSpec player_ends_turn()
        {
            _gameObserverSpy.AttackPhase.EndTurn();
            return this;
        }

        private void player_1_should_occupy_north_africa_with_six_armies()
        {
            AssertTerritory(_regions.NorthAfrica, _player1, 6);
        }

        private void AssertTerritory(IRegion region, IPlayer player, int armies)
        {
            _gameObserverSpy.Game.Territories.Single(x => x.Region == region).Player.Should().Be(player);
            _gameObserverSpy.Game.Territories.Single(x => x.Region == region).Armies.Should().Be(armies);
        }

        private void player_2_should_occupy_brazil_with_4_armies()
        {
            AssertTerritory(_regions.Brazil, _player2, 4);
        }

        private GameObserverSpec player_1_should_occupy_brazil_with_five_armies()
        {
            AssertTerritory(_regions.Brazil, _player1, 5);
            return this;
        }

        private void player_1_should_occupy_north_africa_with_three_armies()
        {
            AssertTerritory(_regions.NorthAfrica, _player1, 3);
        }

        private void east_africa_should_have_two_armies()
        {
            AssertTerritory(_regions.EastAfrica, _player1, 2);
        }

        private void player_1_is_the_winner()
        {
            _gameObserverSpy.GameIsOver.Winner.Should().Be(_player1);
        }

        private void player_1_should_take_turn()
        {
            _gameObserverSpy.Game.CurrentPlayer.Should().Be(_player1);
        }
    }

    internal class GameObserverSpy : IGameObserver
    {
        public IGame Game { get; private set; }
        public IDraftArmiesPhase DraftArmiesPhase { get; private set; }
        public IAttackPhase AttackPhase { get; private set; }
        public ISendArmiesToOccupyPhase SendArmiesToOccupyPhase { get; private set; }
        public IEndTurnPhase EndTurnPhase { get; private set; }
        public IGameIsOver GameIsOver { get; private set; }

        public void NewGame(IGame game)
        {
            Game = game;
        }

        public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            DraftArmiesPhase = draftArmiesPhase;
        }

        public void Attack(IAttackPhase attackPhase)
        {
            AttackPhase = attackPhase;
        }

        public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            SendArmiesToOccupyPhase = sendArmiesToOccupyPhase;
        }

        public void EndTurn(IEndTurnPhase endTurnPhase)
        {
            EndTurnPhase = endTurnPhase;
        }

        public void GameOver(IGameIsOver gameIsOver)
        {
            GameIsOver = gameIsOver;
        }
    }
}