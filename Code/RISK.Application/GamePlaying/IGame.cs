using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public interface IGame
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
}