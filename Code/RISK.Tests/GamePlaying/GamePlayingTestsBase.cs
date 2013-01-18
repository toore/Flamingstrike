﻿using System.Collections.Generic;
using NUnit.Framework;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.EntityProviders;

namespace RISK.Tests.GamePlaying
{
    

    public class GamePlayingTestsBase : AcceptanceTestsBase<InTurnGamePlayingTests>
    {
        private IAreaDefinitionProvider _areaDefinitionProvider;
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _areaDefinitionProvider = new AreaDefinitionProvider(new ContinentProvider());
        }

        protected InTurnGamePlayingTests new_game_with(int humanUsers)
        {
            _game = new Game();

            //CreateHumanUsers(humanUsers).Apply(_game.AddUser);

            return This;
        }

        private IEnumerable<HumanUser> CreateHumanUsers(int humanUsers)
        {
            for (var i = 0; i < humanUsers; i++)
            {
                yield return new HumanUser();
            }
        }

        protected IAreaDefinition Brazil
        {
            get { return _areaDefinitionProvider.Brazil; }
        }

        protected IAreaDefinition NorthAfrica
        {
            get { return _areaDefinitionProvider.NorthAfrica; }
        }
    }
}