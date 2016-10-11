using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.GameEngine;
using RISK.UI.WPF.Properties;
using RISK.UI.WPF.ViewModels.Messages;

namespace RISK.UI.WPF.ViewModels.Preparation
{
    public interface IGamePreparationViewModel : IMainViewModel
    {
        IList<GamePreparationPlayerViewModel> Players { get; }
        void Confirm();
    }

    public class GamePreparationViewModel : ViewModelBase, IGamePreparationViewModel
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayerRepository _playerRepository;

        public GamePreparationViewModel(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IPlayerRepository playerRepository, IEventAggregator eventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _eventAggregator = eventAggregator;
            _playerRepository = playerRepository;

            const int maxNumberOfPlayers = 6;
            Players = Enumerable.Range(0, maxNumberOfPlayers)
                .Select(CreateGamePreparationPlayerViewModel)
                .ToList();
        }

        private GamePreparationPlayerViewModel CreateGamePreparationPlayerViewModel(int playerIndex)
        {
            return new GamePreparationPlayerViewModel(_playerTypes)
                {
                    Name = string.Format(Resources.PLAYER_NUMBER, playerIndex + 1),
                    OnIsEnabledChanged = () => OnEnabledPlayerChanged()
                };
        }

        private void OnEnabledPlayerChanged()
        {
            NotifyOfPropertyChange(() => CanConfirm);
        }

        public IList<GamePreparationPlayerViewModel> Players { get; }

        public bool CanConfirm => GetEnabledPlayers().Count() > 1;

        public void Confirm()
        {
            _playerRepository.Clear();

            foreach (var player in CreatePlayers())
            {
                _playerRepository.Add(player);
            }

            _eventAggregator.PublishOnUIThread(new StartGameSetupMessage());
        }

        private IEnumerable<IPlayer> CreatePlayers()
        {
            return GetEnabledPlayers()
                .Select(vm => _playerFactory.Create(vm.Name));
        }

        private IEnumerable<GamePreparationPlayerViewModel> GetEnabledPlayers()
        {
            return Players
                .Where(x => x.IsEnabled);
        }
    }
}