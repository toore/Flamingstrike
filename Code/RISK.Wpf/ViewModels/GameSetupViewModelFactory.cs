using System;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IPlayerFactory _playerFactory;

        public GameSetupViewModelFactory(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public IGameSetupViewModel Create(Action<GameSetup> confirm)
        {
            return new GameSetupViewModel(_playerFactory, confirm);
        }
    }
}