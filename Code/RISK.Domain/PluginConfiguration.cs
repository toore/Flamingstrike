using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using StructureMap;

namespace RISK.Domain
{
    public class PluginConfiguration
    {
        public static void Configure()
        {
            ObjectFactory.Configure(x =>
                {
                    x.Scan(s =>
                        {
                            s.AssemblyContainingType<IGame>();

                            s.WithDefaultConventions();
                        });

                    x.For<ITerritoryLocationRepository>().Singleton();
                });
        }
    }
}