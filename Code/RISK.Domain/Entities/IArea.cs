namespace RISK.Domain.Entities
{
    public interface IArea
    {
        IAreaDefinition AreaDefinition { get; }
        IPlayer Owner { get; set; }
        int Armies { get; set; }
        bool HasOwner { get; }
    }
}