using System;
using System.Linq;
using GuiWpf.RegionModels;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using RISK.Core;
using RISK.GameEngine;

namespace GuiWpf.Views.Gameplay
{
    public partial class GameboardView
    {
        public GameboardView()
        {
            InitializeComponent();
        }
    }

    public class GameboardViewModelDesignerData : IGameboardViewModel
    {
        public GameboardViewModelDesignerData()
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
                .ToList().AsReadOnly();

            WorldMapViewModel = worldMapViewModelFactory.Create(territories, x => { }, Enumerable.Empty<IRegion>());
        }

        public WorldMapViewModel WorldMapViewModel { get; }
        public string InformationText => "Information text is shown here";

        public string PlayerName => "Player name is shown here";

        public bool CanActivateFreeMove()
        {
            return true;
        }

        public void ActivateFreeMove() {}

        public bool CanEndTurn()
        {
            return true;
        }

        public void EndTurn() {}

        public void EndGame() {}
    }
}