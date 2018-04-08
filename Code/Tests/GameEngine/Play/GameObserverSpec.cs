using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup.Finished;
using FluentAssertions;
using NSubstitute;
using Toore.Shuffling;
using Xunit;
using Territory = FlamingStrike.GameEngine.Setup.Finished.Territory;

namespace Tests.GameEngine.Play
{
    public class GameObserverSpec : SpecBase<GameObserverSpec>
    {
        private Regions _regions;
        private PlayerName _player1;
        private PlayerName _player2;
        private IDie _die;
        private readonly List<Territory> _territories = new List<Territory>();
        private Continents _continents;
        private readonly GameObserverSpy _gameObserverSpy = new GameObserverSpy();

        [Fact]
        public void First_player_draft_armies()
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
        public void First_player_occupies_brazil_after_successfull_attack()
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

            When.player_attacks_brazil_from_north_africa_and_wins();

            Then.player_1_is_the_winner();
        }

        private void player_drafts_armies_in_north_africa()
        {
            _gameObserverSpy.DraftArmiesPhase.PlaceDraftArmies(_regions.NorthAfrica, 1);
        }

        private void player_1_should_have_a_card()
        {
            _gameObserverSpy.PlayerGameDatas
                .ShouldContainSinglePlayerGameData(_player1)
                .Cards.Count().Should().Be(1);
        }

        private void player_1_should_not_have_any_card()
        {
            _gameObserverSpy.PlayerGameDatas
                .ShouldContainSinglePlayerGameData(_player1)
                .Cards.Should().BeEmpty();
        }

        private void player_moves_one_army_from_north_africa_to_east_africa()
        {
            _gameObserverSpy.AttackPhase.Fortify(_regions.NorthAfrica, _regions.EastAfrica, 1);
        }

        private GameObserverSpec a_game_with_two_human_players()
        {
            _continents = new Continents();
            _regions = new Regions(_continents);

            _player1 = new PlayerName("player 1");
            _player2 = new PlayerName("player 2");

            return this;
        }

        private GameObserverSpec game_is_started()
        {
            _die = Substitute.For<IDie>();
            var dice = new Dice(_die);
            var armyDraftCalculator = new ArmyDraftCalculator(_continents);
            var battle = new Battle(dice, new ArmiesLostCalculator());
            var armyDrafter = new ArmyDrafter();
            var territoryOccupier = new TerritoryOccupier();
            var fortifier = new Fortifier();
            var attacker = new Attacker(battle);
            var playerEliminationRules = new PlayerEliminationRules();
            var gamePhaseFactory = new GamePhaseFactory(armyDrafter, attacker, fortifier, playerEliminationRules, territoryOccupier);
            var fisherYatesShuffle = new FisherYatesShuffler(new RandomWrapper());
            var deckFactory = new DeckFactory(_regions, fisherYatesShuffle);
            var gameFactory = new GameBootstrapper(gamePhaseFactory, armyDraftCalculator, deckFactory);

            IReadOnlyList<PlayerName> players = new List<PlayerName>
                {
                    new PlayerName("player 1"),
                    new PlayerName("player 2")
                };
            var gamePlaySetup = new GamePlaySetup(players, _territories);

            gameFactory.Run(_gameObserverSpy, gamePlaySetup);

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

        private void AddTerritoryToGame(IRegion region, PlayerName playerName, int armies)
        {
            _territories.Add(new Territory(region, playerName, armies));
        }

        private GameObserverSpec player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
        {
            GetAllRegionsExcept(
                _regions.Brazil,
                _regions.Venezuela,
                _regions.NorthAfrica).Apply(x => AddTerritoryToGame(x, _player1, 1));

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
                _regions.Brazil).Apply(x => AddTerritoryToGame(x, _player1, 1));
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
            _die.Roll().Returns(6, 6, 6, 1);
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
            _gameObserverSpy.Territories.ShouldContainSingleTerritory(_regions.NorthAfrica)
                .PlayerShouldBe(_player1)
                .ArmiesShouldBe(6);
        }

        private void player_2_should_occupy_brazil_with_4_armies()
        {
            _gameObserverSpy.Territories.ShouldContainSingleTerritory(_regions.Brazil)
                .PlayerShouldBe(_player2)
                .ArmiesShouldBe(4);
        }

        private GameObserverSpec player_1_should_occupy_brazil_with_five_armies()
        {
            _gameObserverSpy.Territories.ShouldContainSingleTerritory(_regions.Brazil)
                .PlayerShouldBe(_player1)
                .ArmiesShouldBe(5);
            return this;
        }

        private void player_1_should_occupy_north_africa_with_three_armies()
        {
            _gameObserverSpy.Territories.ShouldContainSingleTerritory(_regions.NorthAfrica)
                .PlayerShouldBe(_player1)
                .ArmiesShouldBe(3);
        }

        private void east_africa_should_have_two_armies()
        {
            _gameObserverSpy.Territories.ShouldContainSingleTerritory(_regions.EastAfrica)
                .PlayerShouldBe(_player1)
                .ArmiesShouldBe(2);
        }

        private void player_1_is_the_winner()
        {
            _gameObserverSpy.GameOverState.Winner.Should().Be(_player1);
        }

        private void player_1_should_take_turn()
        {
            _gameObserverSpy.CurrentPlayerName.Should().Be(_player1);
        }
    }

    internal class GameObserverSpy : IGameObserver
    {
        private object _phase;

        public PlayerName CurrentPlayerName { get; private set; }
        public IReadOnlyList<ITerritory> Territories { get; private set; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; private set; }

        public IDraftArmiesPhase DraftArmiesPhase => (IDraftArmiesPhase)_phase;
        public IAttackPhase AttackPhase => (IAttackPhase)_phase;
        public ISendArmiesToOccupyPhase SendArmiesToOccupyPhase => (ISendArmiesToOccupyPhase)_phase;
        public IEndTurnPhase EndTurnPhase => (IEndTurnPhase)_phase;
        public IGameOverState GameOverState => (IGameOverState)_phase;

        public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            CurrentPlayerName = draftArmiesPhase.CurrentPlayerName;
            Territories = draftArmiesPhase.Territories;
            PlayerGameDatas = draftArmiesPhase.PlayerGameDatas;
            _phase = draftArmiesPhase;
        }

        public void Attack(IAttackPhase attackPhase)
        {
            CurrentPlayerName = attackPhase.CurrentPlayerName;
            Territories = attackPhase.Territories;
            PlayerGameDatas = attackPhase.PlayerGameDatas;
            _phase = attackPhase;
        }

        public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            CurrentPlayerName = sendArmiesToOccupyPhase.CurrentPlayerName;
            Territories = sendArmiesToOccupyPhase.Territories;
            PlayerGameDatas = sendArmiesToOccupyPhase.PlayerGameDatas;
            _phase = sendArmiesToOccupyPhase;
        }

        public void EndTurn(IEndTurnPhase endTurnPhase)
        {
            CurrentPlayerName = endTurnPhase.CurrentPlayerName;
            Territories = endTurnPhase.Territories;
            PlayerGameDatas = endTurnPhase.PlayerGameDatas;
            _phase = endTurnPhase;
        }

        public void GameOver(IGameOverState gameOverState)
        {
            CurrentPlayerName = null;
            Territories = null;
            PlayerGameDatas = null;
            _phase = gameOverState;
        }
    }

    internal class TerritoryAssertion
    {
        private readonly ITerritory _territory;

        public TerritoryAssertion(ITerritory territory)
        {
            _territory = territory;
        }

        public TerritoryAssertion PlayerShouldBe(PlayerName playerName)
        {
            _territory.Name.Should().Be(playerName);
            return this;
        }

        public TerritoryAssertion ArmiesShouldBe(int armies)
        {
            _territory.Armies.Should().Be(armies);
            return this;
        }
    }

    internal static class GameStatusExtensions
    {
        public static IPlayerGameData ShouldContainSinglePlayerGameData(this IReadOnlyList<IPlayerGameData> playerGameDatas, PlayerName playerName)
        {
            playerGameDatas.Should().ContainSingle(x => x.PlayerName == playerName);
            return playerGameDatas.Single(x => x.PlayerName == playerName);
        }

        public static TerritoryAssertion ShouldContainSingleTerritory(this IReadOnlyList<ITerritory> territories, IRegion region)
        {
            territories.Should().ContainSingle(x => x.Region == region);
            return new TerritoryAssertion(territories.Single(x => x.Region == region));
        }
    }
}