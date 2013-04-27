namespace RISK.Base
{
    public class Maybe<T>
    {
        private readonly T _value;

        public static readonly Maybe<T> Nothing = new Maybe<T>();

        public Maybe(T value)
        {
            _value = value;
        }

        private Maybe() {}

        public bool HasValue
        {
            get { return this != Nothing; }
        }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new ForbiddenMaybeValueAccessException("There is no value to read!");
                }

                return _value;
            }
        }
    }
}