using Caliburn.Micro;
using GuiWpf.Views.WorldMap;
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

                            s.AssemblyContainingType<IWindowManager>();

                            s.WithDefaultConventions();
                        });

                    x.For<ILocationRepository>().Singleton();
                });
        }
    }
}