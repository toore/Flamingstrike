using System;

// Maybe monad, by courtesy of Robert Gustavsson and Chris Jansson

namespace FlamingStrike.Core
{
    public class Maybe<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        private Maybe(T value)
        {
            _hasValue = true;
            _value = value;
        }

        private Maybe()
        {
            _hasValue = false;
        }

        public Maybe<TU> Bind<TU>(Func<T, TU> bind)
        {
            return _hasValue ? new Maybe<TU>(bind(_value)) : Maybe<TU>.Nothing;
        }

        public Maybe<TU> Map<TU>(Func<T, Maybe<TU>> mapper)
        {
            return _hasValue ? mapper(_value) : Maybe<TU>.Nothing;
        }

        public void End(Action<T> onSuccess, Action onFailure)
        {
            if (_hasValue)
                onSuccess(_value);
            else
                onFailure();
        }

        public TU Fold<TU>(Func<T, TU> onSuccess, Func<TU> onFailure)
        {
            return _hasValue ? onSuccess(_value) : onFailure();
        }

        public static readonly Maybe<T> Nothing = new Maybe<T>();

        public static Maybe<T> Create(T value)
        {
            return new Maybe<T>(value);
        }
    }
}