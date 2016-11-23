using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.RegionModels;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.Views.Gameplay
{
    public partial class GameplayView
    {
        public GameplayView()
        {
            InitializeComponent();
        }
    }

    public class GameplayViewModelDesignerData
    {
        public GameplayViewModelDesignerData()
        {
            var continents = new Continents();
            var regions = new Regions(continents);
            var regionModelFactory = new RegionModelFactory(regions);
            var playerUiDataRepository = new PlayerUiDataRepository();
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, playerUiDataRepository);

            var random = new Random();

            Player[] players = { new Player("player one"), new Player("player two") };
            playerUiDataRepository.Add(new PlayerUiData(players[0], Colors.DeepPink));
            playerUiDataRepository.Add(new PlayerUiData(players[1], Colors.Teal));
            var playersBuffer = new CircularBuffer<Player>(players);

            var territories = regions.GetAll()
                .Select(region => new Territory(region, playersBuffer.Next(), random.Next(99) + 1))
                .ToList();

            WorldMapViewModel = worldMapViewModelFactory.Create(x => { });
            worldMapViewModelFactory.Update(WorldMapViewModel, territories, Enumerable.Empty<IRegion>().ToList(), Maybe<IRegion>.Create(regions.Iceland));
        }

        public WorldMapViewModel WorldMapViewModel { get; }

        public string InformationText => "Information text is shown here";

        public string PlayerName => "Player name is shown here";

        public bool CanEnterFortifyMode => false;

        public bool CanEnterAttackMode => true;

        public bool CanEndTurn => true;

        public Color PlayerColor => Colors.DeepPink;

        public IList<PlayerStatusViewModel> Players => new List<PlayerStatusViewModel>()
            {
                new PlayerStatusViewModel("Player 1", Colors.Aqua, 5),
                new PlayerStatusViewModel("Player 2", Colors.Violet, 0),
                new PlayerStatusViewModel("Player 3", Colors.YellowGreen, 1),
                new PlayerStatusViewModel("Player 4", Colors.Tomato, 2),
                new PlayerStatusViewModel("Player 5", Colors.LemonChiffon, 1),
                new PlayerStatusViewModel("Player 6", Colors.RosyBrown, 0),
            };
    }
}