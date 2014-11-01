namespace RISK.Domain.Entities
{
    public interface ILocation
    {
        string Name { get; }
        Continent Continent { get; }
        bool IsBordering(ILocation location);
    }
}