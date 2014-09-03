namespace RISK.Tests.Application.Specifications
{
    public class AcceptanceTestsBase<T> where T : AcceptanceTestsBase<T>
    {
        protected T Given
        {
            get { return (T)this; }
        }

        protected T When
        {
            get { return (T)this; }
        }

        protected T Then
        {
            get { return (T)this; }
        }
    }
}