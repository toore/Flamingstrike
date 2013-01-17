using System.Collections.Generic;
using Caliburn.Micro;
using NUnit.Framework;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.EntityProviders;

namespace RISK.Tests.GamePlaying
{
    public class GamePlayingTestsBase : AcceptanceTestsBase<InTurnGamePlayingTests>
    {
        private IAreaProvider _areaProvider;
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _areaProvider = new AreaProvider(new ContinentProvider());
        }

        protected InTurnGamePlayingTests new_game_with(int humanUsers)
        {
            _game = new Game();

            CreateHumanUsers(humanUsers).Apply(_game.AddUser);

            return This;
        }

        private IEnumerable<HumanUser> CreateHumanUsers(int humanUsers)
        {
            for (var i = 0; i < humanUsers; i++)
            {
                yield return new HumanUser();
            }
        }

        protected IArea Brazil
        {
            get { return _areaProvider.Brazil; }
        }

        protected IArea NorthAfrica
        {
            get { return _areaProvider.NorthAfrica; }
        }
    }
}