using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public interface ITerritoryLocationRepository
    {
        IEnumerable<ITerritoryLocation> GetAll();

        ITerritoryLocation Alaska { get; }
        ITerritoryLocation Alberta { get; }
        ITerritoryLocation CentralAmerica { get; }
        ITerritoryLocation EasternUnitedStates { get; }
        ITerritoryLocation Greenland { get; }
        ITerritoryLocation NorthwestTerritory { get; }
        ITerritoryLocation Ontario { get; }
        ITerritoryLocation Quebec { get; }
        ITerritoryLocation WesternUnitedStates { get; }
        ITerritoryLocation Argentina { get; }
        ITerritoryLocation Brazil { get; }
        ITerritoryLocation Peru { get; }
        ITerritoryLocation Venezuela { get; }
        ITerritoryLocation GreatBritain { get; }
        ITerritoryLocation Iceland { get; }
        ITerritoryLocation NorthernEurope { get; }
        ITerritoryLocation Scandinavia { get; }
        ITerritoryLocation SouthernEurope { get; }
        ITerritoryLocation Ukraine { get; }
        ITerritoryLocation WesternEurope { get; }
        ITerritoryLocation Congo { get; }
        ITerritoryLocation EastAfrica { get; }
        ITerritoryLocation Egypt { get; }
        ITerritoryLocation Madagascar { get; }
        ITerritoryLocation NorthAfrica { get; }
        ITerritoryLocation SouthAfrica { get; }
        ITerritoryLocation Afghanistan { get; }
        ITerritoryLocation China { get; }
        ITerritoryLocation India { get; }
        ITerritoryLocation Irkutsk { get; }
        ITerritoryLocation Japan { get; }
        ITerritoryLocation Kamchatka { get; }
        ITerritoryLocation MiddleEast { get; }
        ITerritoryLocation Mongolia { get; }
        ITerritoryLocation Siam { get; }
        ITerritoryLocation Siberia { get; }
        ITerritoryLocation Ural { get; }
        ITerritoryLocation Yakutsk { get; }
        ITerritoryLocation EasternAustralia { get; }
        ITerritoryLocation Indonesia { get; }
        ITerritoryLocation NewGuinea { get; }
        ITerritoryLocation WesternAustralia { get; }
    }
}