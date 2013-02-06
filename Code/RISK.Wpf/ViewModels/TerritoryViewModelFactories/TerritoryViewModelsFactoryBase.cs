using System.Windows;
using GuiWpf.Services;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public abstract class TerritoryViewModelsFactoryBase : ITerritoryViewModelsFactory
    {
        private readonly ITerritoryColors _territoryColors;

        protected TerritoryViewModelsFactoryBase(ITerritoryColors territoryColors)
        {
            _territoryColors = territoryColors;
        }

        public TextViewModel CreateTextViewModel(ITerritory territory)
        {
            return new TextViewModel
                {
                    Position = NamePosition,
                    TerritoryName = Name,
                    Armies = territory.Armies
                };
        }

        public TerritoryViewModel CreateTerritoryViewModel()
        {
            return new TerritoryViewModel
                {
                    Path = Path,
                    NormalStrokeColor = _territoryColors.NormalStrokeColor,
                    NormalFillColor = _territoryColors.NormalFillColor,
                    MouseOverStrokeColor = _territoryColors.MouseOverStrokeColor,
                    MouseOverFillColor = _territoryColors.MouseOverFillColor
                };
        }

        protected abstract string Name { get; }
        protected abstract Point NamePosition { get; }
        protected abstract string Path { get; }
    }
}