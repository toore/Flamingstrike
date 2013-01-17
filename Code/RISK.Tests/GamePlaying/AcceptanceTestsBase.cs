namespace RISK.Tests.GamePlaying
{
    public class AcceptanceTestsBase<T> where T : AcceptanceTestsBase<T>
    {
        protected T Given
        {
            get { return This; }
        }

        protected T When
        {
            get { return This; }
        }

        protected T User
        {
            get { return This; }
        }

        protected T Then
        {
            get { return This; }
        }

        protected T This
        {
            get { return (T)this; }
        }
    }
}