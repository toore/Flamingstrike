using System;
using GuiWpf.TerritoryModels;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryTextViewModelFactory
    {
        ITerritoryTextViewModel Create(ITerritory territory);
    }

    public class TerritoryTextViewModelFactory : ITerritoryTextViewModelFactory
    {
        private readonly IWorldMapModelFactory _worldMapModelFactory;

        public TerritoryTextViewModelFactory(IWorldMapModelFactory worldMapModelFactory)
        {
            _worldMapModelFactory = worldMapModelFactory;
        }

        public ITerritoryTextViewModel Create(ITerritory territory)
        {
            throw new NotSupportedException();
            //var territoryModel = _worldMapModelFactory.Create(territory);

            //return new TerritoryTextViewModel
            //    {
            //        Territory = territory,
            //        TerritoryName = territoryModel.Name,
            //        Position = territoryModel.NamePosition,
            //        Armies = territory.Armies
            //    };
        }
    }
}