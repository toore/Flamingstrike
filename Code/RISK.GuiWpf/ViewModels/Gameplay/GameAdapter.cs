using System.Collections.Generic;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameAdapter
    {
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

        private void MoveToNextPlayer()
        {
            _stateController = _stateControllerFactory.Create(_game);
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
            _stateController.CurrentState = _interactionStateFactory.CreateFortifyState(_stateController);
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