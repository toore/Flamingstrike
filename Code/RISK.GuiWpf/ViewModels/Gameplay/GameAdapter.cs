using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameAdapter
    {
        IWorldMap WorldMap { get; }
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }
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
        private readonly Game _game;
        private IStateController _stateController;

        public GameAdapter(IInteractionStateFactory interactionStateFactory, IStateControllerFactory stateControllerFactory, Game game)
        {
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
            _game = game;

            MoveToNextPlayer();
        }

        public IWorldMap WorldMap => _game.WorldMap;
        public IPlayer Player => _game.Player;
        public ITerritory SelectedTerritory => _stateController.CurrentState.SelectedTerritory;

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