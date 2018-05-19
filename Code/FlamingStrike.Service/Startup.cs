using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Toore.Shuffling;

namespace FlamingStrike.Service
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddGameEngineHub();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR(
                routes => { routes.MapHub<GameEngineHub>("/hubs/gameengine"); });
        }
    }

    public static class AddGameEngineHubHelper
    {
        public static void AddGameEngineHub(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddSingleton(
                sp =>
                    {
                        var randomWrapper = new RandomWrapper();
                        var shuffler = new FisherYatesShuffler(randomWrapper);
                        var worldMapFactory = new WorldMapFactory();
                        var worldMap = worldMapFactory.Create();
                        var deckFactory = new DeckFactory(worldMap.GetAll(), shuffler);
                        var armiesLostCalculator = new ArmiesLostCalculator();
                        var die = new Die(randomWrapper);
                        var dice = new Dice(die);
                        var attackService = new AttackService(worldMap, dice, armiesLostCalculator);
                        var playerEliminationRules = new PlayerEliminationRules();
                        var armyDraftCalculator = new ArmyDraftCalculator(new[] { Continent.Asia, Continent.NorthAmerica, Continent.Europe, Continent.Africa, Continent.Australia, Continent.SouthAmerica });
                        var gamePhaseFactory = new GamePhaseFactory(attackService, playerEliminationRules, worldMap);

#if DEBUG_QUICKSETUP
                        var startingInfantryCalculator = new StartingInfantryCalculatorReturning22Armies();
#else
                        var startingInfantryCalculator = new StartingInfantryCalculator();
#endif

                        var alternateGameSetupBootstrapper = new AlternateGameSetupBootstrapper(worldMap.GetAll(), shuffler, startingInfantryCalculator);
                        var gameBootstrapper = new GameBootstrapper(gamePhaseFactory, armyDraftCalculator, deckFactory);

                        return new GameEngineHub(alternateGameSetupBootstrapper, gameBootstrapper);
                    });
        }
    }

    public class StartingInfantryCalculatorReturning22Armies : IStartingInfantryCalculator
    {
        public int Get(int numberOfPlayers)
        {
            return 22;
        }
    }
}