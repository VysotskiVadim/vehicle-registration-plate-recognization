using System;
using NullGuard;

namespace vrpr.Core.Infrastructure
{
    public struct Maybe<T>
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (HasNoValue)
                {
                    throw new NotSupportedException("Maybe has not value");
                }

                return _value;
            }
        }

        public bool HasValue
        {
            get { return _value != null; }
        }

        public bool HasNoValue => !HasValue;

        private Maybe([AllowNull] T value)
        {
            _value = value;
        }

        public static implicit operator Maybe<T>([AllowNull] T value)
        {
            return new Maybe<T>(value);
        }
    }
}
