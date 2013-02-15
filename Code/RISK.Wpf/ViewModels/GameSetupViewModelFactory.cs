using System;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;

        public GameSetupViewModelFactory(IPlayerFactory playerFactory, IPlayerTypes playerTypes)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
        }

        public IGameSetupViewModel Create(Action<GameSetup> confirm)
        {
            return new GameSetupViewModel(_playerFactory, _playerTypes, confirm);
        }
    }
}