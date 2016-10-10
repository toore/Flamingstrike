using System;
using System.Linq;
using RISK.GameEngine;
using RISK.UI.WPF.RegionModels;
using RISK.UI.WPF.Services;
using RISK.UI.WPF.ViewModels.Gameplay;

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
            var colorService = new ColorService();
            var regionColorSettingFactory = new RegionColorSettingsFactory(colorService, regions);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, regionColorSettingFactory, colorService);

            var random = new Random();

            var territories = regions.GetAll()
                .Select(region => new Territory(region, new Player("player"), random.Next(99) + 1))
                .ToList();

            WorldMapViewModel = worldMapViewModelFactory.Create(x => { });
            worldMapViewModelFactory.Update(WorldMapViewModel, territories, Enumerable.Empty<IRegion>().ToList(), Maybe<IRegion>.Nothing);
        }

        public WorldMapViewModel WorldMapViewModel { get; }

        public string InformationText => "Information text is shown here";

        public string PlayerName => "Player name is shown here";

        public bool CanEnterFortifyMode => false;

        public bool CanEnterAttackMode => true;

        public bool CanEndTurn => true;
    }
}