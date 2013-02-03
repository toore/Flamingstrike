using GuiWpf.Views.Main;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using StructureMap;

namespace GuiWpf.Infrastructure
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
                            s.AssemblyContainingType<IGameEngine>();

                            s.WithDefaultConventions();
                        });

                    x.For<ILocationRepository>().Singleton();
                });
        }
    }
}