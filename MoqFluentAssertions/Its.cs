using FluentAssertions;

namespace MoqFluentAssertions
{
    public static class Its
    {
        /// <summary>
        /// Note: can only be used if there is only one call to the method being verified
        /// </summary>
        public static T EquivalentTo<T>(T expected)
        {
            return Moq.It.Is<T>(actual => actual.ShouldBeEquivalentToReturnSuccess(expected));
        }

        /// <summary>
        /// Note: can only be used if there is only one call to the method being verified
        /// </summary>
        public static TActual EquivalentTo<TActual, TExpected>(TExpected expected)
        {
            return Moq.It.Is<TActual>(actual => actual.ShouldBeEquivalentToReturnSuccess(expected));
        }

        /// <summary>
        /// Calls fuent assert should be equivilent to and returns true or throws an assertion exception details the property mismatch.
        /// For use in Moq verify 
        /// e.g. mock.Verify(t => s.TomeMethod(Moq.It.Is(o=>o.ShouldBeEquivalentToReturnSuccess(expected))))
        /// </summary>
        /// <typeparam name="T1Value"></typeparam>
        /// <typeparam name="T2Value"></typeparam>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static bool ShouldBeEquivalentToReturnSuccess<T1Value, T2Value>(this T1Value expected, T2Value actual)
        {
            actual.ShouldBeEquivalentTo(expected);
            return true;
        }
    }
}
