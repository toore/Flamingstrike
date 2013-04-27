using System;

namespace RISK.Base
{
    public class ForbiddenMaybeValueAccessException : Exception
    {
        public ForbiddenMaybeValueAccessException(string message) : base(message) { }
    }
}