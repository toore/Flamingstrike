namespace RISK.Tests.Application.Specifications
{
    public class SpecBase<T> where T : SpecBase<T>
    {
        protected T Given => (T)this;
        protected T When => (T)this;
        protected T Then => (T)this;
    }
}