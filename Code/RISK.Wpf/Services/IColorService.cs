namespace GuiWpf.Services
{
    public interface IColorService
    {
        ContinentColors NorthAmericaColors { get; }
        ContinentColors SouthAmericaColors { get; }
        ContinentColors EuropeColors { get; }
        ContinentColors AfricaColors { get; }
        ContinentColors AsiaColors { get; }
        ContinentColors AustraliaColors { get; }
    }
}