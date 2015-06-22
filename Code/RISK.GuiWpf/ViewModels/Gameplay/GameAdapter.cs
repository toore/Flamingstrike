using System.Collections.Generic;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameAdapter
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }
        IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }
        void EndTurn();
        bool IsGameOver();
        void Fortify();
        void OnClick(ITerritory territory);
        bool CanClick(ITerritory territory);
    }

    public class GameAdapter : IGameAdapter
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly IGame _game;
        private IStateController _stateController;

        public GameAdapter(IInteractionStateFactory interactionStateFactory, IStateControllerFactory stateControllerFactory, IGame game)
        {
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
            _game = game;

            MoveToNextPlayer();
        }

        public IPlayer Player => _game.CurrentPlayer;
        public ITerritory SelectedTerritory => _stateController.CurrentState.SelectedTerritory;
        public IReadOnlyList<IGameboardTerritory> GameboardTerritories => _game.Territories;

        private void MoveToNextPlayer()
        {
            _stateController = _stateControllerFactory.Create(Player, _game);
            _stateController.SetInitialState();
        }

        public void EndTurn()
        {
            _game.EndTurn();

            MoveToNextPlayer();
        }

        public bool IsGameOver()
        {
            return _game.IsGameOver();
        }

        public void Fortify()
        {
            _stateController.CurrentState = _interactionStateFactory.CreateFortifyState(_stateController, Player);
        }

        public void OnClick(ITerritory territory)
        {
            _stateController.CurrentState.OnClick(territory);
        }

        public bool CanClick(ITerritory territory)
        {
            return _stateController.CurrentState.CanClick(territory);
        }
    }
}