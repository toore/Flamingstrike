using System;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypesFactory _playerTypesFactory;

        public GameSetupViewModelFactory(IPlayerFactory playerFactory, IPlayerTypesFactory playerTypesFactory)
        {
            _playerFactory = playerFactory;
            _playerTypesFactory = playerTypesFactory;
        }

        public IGameSetupViewModel Create(Action<GameSetup> confirm)
        {
            return new GameSetupViewModel(_playerFactory, _playerTypesFactory, confirm);
        }
    }
}