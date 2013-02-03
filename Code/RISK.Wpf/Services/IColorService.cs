namespace GuiWpf.Services
{
    public interface IColorService
    {
        ITerritoryColors NorthAmericaColors { get; }
        ITerritoryColors SouthAmericaColors { get; }
        ITerritoryColors EuropeColors { get; }
        ITerritoryColors AfricaColors { get; }
        ITerritoryColors AsiaColors { get; }
        ITerritoryColors AustraliaColors { get; }
    }
}