namespace RISK.Base.Extensions
{
    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybeWhenNotNull<T>(this T value)
            where T : class
        {
            return value == null ? Maybe<T>.Nothing : new Maybe<T>(value);
        } 
    }
}