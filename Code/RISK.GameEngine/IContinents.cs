using System.Collections.Generic;

namespace RISK.Core
{
    public interface IContinents
    {
        IEnumerable<IContinent> GetAll();

        IContinent Asia { get; }
        IContinent NorthAmerica { get; }
        IContinent Europe { get; }
        IContinent Africa { get; }
        IContinent Australia { get; }
        IContinent SouthAmerica { get; }
    }
}