using System;
using GuiWpf.GuiDefinitions;
using GuiWpf.Services;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.WorldMapViewModels
{
    public class TerritoryViewModelFactory : ITerritoryViewModelFactory
    {
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly ITerritoryLayoutInformationFactory _territoryLayoutInformationFactory;

        public TerritoryViewModelFactory(ITerritoryColorsFactory territoryColorsFactory, ITerritoryLayoutInformationFactory territoryLayoutInformationFactory)
        {
            _territoryColorsFactory = territoryColorsFactory;
            _territoryLayoutInformationFactory = territoryLayoutInformationFactory;
        }

        public TerritoryViewModel Create(ITerritory territory, Action<ILocation> clickCommand)
        {
            var layoutInformation = _territoryLayoutInformationFactory.Create(territory.Location);

            var territoryViewModel = new TerritoryViewModel(territory.Location, layoutInformation.Path, clickCommand);

            var territoryColors = _territoryColorsFactory.Create(territory);
            territoryViewModel.NormalStrokeColor = territoryColors.NormalStrokeColor;
            territoryViewModel.NormalFillColor = territoryColors.NormalFillColor;
            territoryViewModel.MouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            territoryViewModel.MouseOverFillColor = territoryColors.MouseOverFillColor;

            return territoryViewModel;
        }
    }
}