using System;
using GuiWpf.Services;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
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

        public TerritoryViewModel Create(ITerritory territory, Action clickCommand)
        {
            var territoryColors = _territoryColorsFactory.Create(territory);

            var layoutInformation = _territoryLayoutInformationFactory.Create(territory.Location);

            return new TerritoryViewModel
                {
                    Path = layoutInformation.Path,
                    NormalStrokeColor = territoryColors.NormalStrokeColor,
                    NormalFillColor = territoryColors.NormalFillColor,
                    MouseOverStrokeColor = territoryColors.MouseOverStrokeColor,
                    MouseOverFillColor = territoryColors.MouseOverFillColor,
                    ClickCommand = clickCommand,
                    IsEnabled = true
                };
        }
    }
}