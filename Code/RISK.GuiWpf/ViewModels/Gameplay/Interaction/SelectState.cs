using System;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class SelectState : IInteractionState
    {
        private readonly IStateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public SelectState(IStateController stateController, IInteractionStateFactory interactionStateFactory, IPlayer player)
        {
            Player = player;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
        }

        public ITerritory SelectedTerritory => null;

        public IPlayer Player { get; }

        public bool CanClick(ITerritory territory)
        {
            var gameboardTerritory = _stateController.Game.Territories.Get(territory);
            return gameboardTerritory.Player == Player;
        }

        public void OnClick(ITerritory territory)
        {
            if (!CanClick(territory))
            {
                throw new InvalidOperationException();
            }

            _stateController.CurrentState = _interactionStateFactory.CreateAttackState(_stateController, Player, territory);
        }
    }
}