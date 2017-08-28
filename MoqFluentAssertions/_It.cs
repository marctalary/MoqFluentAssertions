using Moq;

namespace MoqFluentAssertions
{
    /// <summary>
    /// Use the standard approach
    /// </summary>
    public static class _It
    {
        [Matcher]
        public static T IsEquivalentTo<T>(T expected)
        {
            return default(T);  // Return irrelevant
        }

        [Matcher]
        public static T1 IsEquivalentTo<T1,T2>(T2 expected)
        {
            return default(T1);  // Return irrelevant
        }

        public static bool IsEquivalentTo<T>(T actual, T expected)
        {
            return actual.ShouldBeEquivalentToReturnSuccess(expected, true);
        }

        public static bool IsEquivalentTo<T1,T2>(T1 actual, T2 expected)
        {
            return actual.ShouldBeEquivalentToReturnSuccess(expected, true);
        }
    }


    public static class It<TActual>
    {
        [Matcher]
        public static TActual IsEquivalentTo<TExpected>(TExpected expected)
        {
            return default(TActual);  // Return irrelevant
        }

        public static bool IsEquivalentTo<TExpected>(TActual actual, TExpected expected)
        {
            return actual.ShouldBeEquivalentToReturnSuccess(expected, true);
        }
    }
}
