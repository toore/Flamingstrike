namespace RISK.Domain.Entities
{
    public interface IArea
    {
        IUser Owner { get; set; }
        int Armies { get; set; }
        bool HasOwner { get; }
    }
}