using System;
using System.Linq;
using System.Windows.Media;
using RISK.GameEngine;
using RISK.UI.WPF.RegionModels;
using RISK.UI.WPF.ViewModels.Gameplay;
using RISK.UI.WPF.ViewModels.Preparation;

namespace RISK.UI.WPF.Views.Gameplay
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
    }
}