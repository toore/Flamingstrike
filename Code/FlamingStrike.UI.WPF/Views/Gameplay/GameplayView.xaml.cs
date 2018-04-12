using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FlamingStrike.Core;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.RegionModels;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using Territory = FlamingStrike.UI.WPF.ViewModels.Gameplay.Territory;

namespace FlamingStrike.UI.WPF.Views.Gameplay
{
    public partial class GameplayView
    {
        public GameplayView()
        {
            InitializeComponent();
        }
    }

    public class GameplayViewModelDesignerData : IGameplayViewModel
    {
        public GameplayViewModelDesignerData()
        {
            var regionModelFactory = new RegionModelFactory();
            var playerUiDataRepository = new PlayerUiDataRepository();
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, playerUiDataRepository);

            var random = new Random();

            PlayerName[] players = { new PlayerName("player one"), new PlayerName("player two") };
            playerUiDataRepository.Add(new PlayerUiData((string)players[0], Colors.DeepPink));
            playerUiDataRepository.Add(new PlayerUiData((string)players[1], Colors.Teal));

            var regions = new WorldMapFactory().Create().GetAll();

            var territories = regions
                .Select((region, i) => new Territory(region, true, players[i % players.Length], random.Next(99) + 1))
                .ToList();

            WorldMapViewModel = worldMapViewModelFactory.Create(x => {});
            worldMapViewModelFactory.Update(WorldMapViewModel, territories, Maybe<Region>.Create(Region.Iceland));
        }

        public WorldMapViewModel WorldMapViewModel { get; }

        public string InformationText => "Information text is shown here";

        public string PlayerName => "Player name is shown here";

        public bool CanShowCards { get; }
        public bool CanEnterFortifyMode => false;

        public bool CanEnterAttackMode => true;

        public bool CanEndTurn => true;

        public Color PlayerColor => Colors.DeepPink;

        public IList<PlayerStatusViewModel> PlayerStatuses => new List<PlayerStatusViewModel>()
            {
                new PlayerStatusViewModel("Player 1", Colors.Aqua, 10),
                new PlayerStatusViewModel("Player 2", Colors.Violet, 2),
                new PlayerStatusViewModel("Player 3", Colors.YellowGreen, 2),
                new PlayerStatusViewModel("Player 4", Colors.Tomato, 0),
                new PlayerStatusViewModel("Player 5", Colors.LemonChiffon, 1),
                new PlayerStatusViewModel("Player 6", Colors.RosyBrown, 5),
            };

        public int NumberOfUserSelectedArmies
        {
            get => 10;
            set {}
        }
        public int MaxNumberOfUserSelectableArmies => 9;

        public bool CanUserSelectNumberOfArmies => true;

        public void EnterAttackMode()
        {
            throw new NotImplementedException();
        }

        public void EnterFortifyMode()
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();
        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }

        public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            throw new NotImplementedException();
        }

        public void Attack(IAttackPhase attackPhase)
        {
            throw new NotImplementedException();
        }

        public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            throw new NotImplementedException();
        }

        public void EndTurn(IEndTurnPhase endTurnPhase)
        {
            throw new NotImplementedException();
        }

        public void GameOver(IGameOverState gameOverState)
        {
            throw new NotImplementedException();
        }
    }
}