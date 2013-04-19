using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public interface IContinentProvider
    {
        Continent NorthAmerica { get; }
        Continent SouthAmerica { get; }
        Continent Europe { get; }
        Continent Africa { get; }
        Continent Asia { get; }
        Continent Australia { get; }
    }
}