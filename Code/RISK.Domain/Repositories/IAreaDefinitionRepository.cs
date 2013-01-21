using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public interface IAreaDefinitionRepository
    {
        IEnumerable<IAreaDefinition> GetAll();

        IAreaDefinition Alaska { get; }
        IAreaDefinition Alberta { get; }
        IAreaDefinition CentralAmerica { get; }
        IAreaDefinition EasternUnitedStates { get; }
        IAreaDefinition Greenland { get; }
        IAreaDefinition NorthwestTerritory { get; }
        IAreaDefinition Ontario { get; }
        IAreaDefinition Quebec { get; }
        IAreaDefinition WesternUnitedStates { get; }
        IAreaDefinition Argentina { get; }
        IAreaDefinition Brazil { get; }
        IAreaDefinition Peru { get; }
        IAreaDefinition Venezuela { get; }
        IAreaDefinition GreatBritain { get; }
        IAreaDefinition Iceland { get; }
        IAreaDefinition NorthernEurope { get; }
        IAreaDefinition Scandinavia { get; }
        IAreaDefinition SouthernEurope { get; }
        IAreaDefinition Ukraine { get; }
        IAreaDefinition WesternEurope { get; }
        IAreaDefinition Congo { get; }
        IAreaDefinition EastAfrica { get; }
        IAreaDefinition Egypt { get; }
        IAreaDefinition Madagascar { get; }
        IAreaDefinition NorthAfrica { get; }
        IAreaDefinition SouthAfrica { get; }
        IAreaDefinition Afghanistan { get; }
        IAreaDefinition China { get; }
        IAreaDefinition India { get; }
        IAreaDefinition Irkutsk { get; }
        IAreaDefinition Japan { get; }
        IAreaDefinition Kamchatka { get; }
        IAreaDefinition MiddleEast { get; }
        IAreaDefinition Mongolia { get; }
        IAreaDefinition Siam { get; }
        IAreaDefinition Siberia { get; }
        IAreaDefinition Ural { get; }
        IAreaDefinition Yakutsk { get; }
        IAreaDefinition EasternAustralia { get; }
        IAreaDefinition Indonesia { get; }
        IAreaDefinition NewGuinea { get; }
        IAreaDefinition WesternAustralia { get; }
    }
}