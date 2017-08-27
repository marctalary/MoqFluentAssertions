using FluentAssertions;
using System;

namespace MoqFluentAssertions
{
    public static class Its
    {
        /// <summary>
        /// Note: can only be used if there is only one call to the method being verified
        /// </summary>
        public static T EquivalentTo<T>(T expected)
        {
            return Moq.It.Is<T>(actual => actual.ShouldBeEquivalentToReturnSuccess(expected, false));
        }

        /// <summary>
        /// Note: can only be used if there is only one call to the method being verified
        /// </summary>
        public static T EquivalentTo<T>(T expected, bool allowMultipleSetups)
        {
            return Moq.It.Is<T>(actual => actual.ShouldBeEquivalentToReturnSuccess(expected, allowMultipleSetups));
        }

        /// <summary>
        /// Note: can only be used if there is only one call to the method being verified
        /// </summary>
        public static TActual EquivalentTo<TActual, TExpected>(TExpected expected)
        {
            return EquivalentTo<TActual, TExpected>(expected, false);
        }

        /// <summary>
        /// Note: can only be used if there is only one call to the method being verified
        /// </summary>
        public static TActual EquivalentTo<TActual, TExpected>(TExpected expected, bool allowMultipleSetups)
        {
            return Moq.It.Is<TActual>(actual => actual.ShouldBeEquivalentToReturnSuccess(expected, allowMultipleSetups));
        }

        /// <summary>
        /// Calls fuent assert should be equivilent to and returns true or throws an assertion exception details the property mismatch.
        /// For use in Moq verify 
        /// e.g. mock.Verify(t => s.TomeMethod(Moq.It.Is(o=>o.ShouldBeEquivalentToReturnSuccess(expected))))
        /// </summary>
        public static bool ShouldBeEquivalentToReturnSuccess<T1Value, T2Value>(this T1Value actual, T2Value expected)
        {
            return actual.ShouldBeEquivalentToReturnSuccess(expected, false);
        }

        /// <summary>
        /// Calls fuent assert should be equivilent to and returns true or throws an assertion exception details the property mismatch.
        /// For use in Moq verify 
        /// e.g. mock.Verify(t => s.TomeMethod(Moq.It.Is(o=>o.ShouldBeEquivalentToReturnSuccess(expected))))
        /// </summary>
        public static bool ShouldBeEquivalentToReturnSuccess<T1Value, T2Value>(this T1Value actual, T2Value expected, bool allowMultipleSetups)
        {
            if (allowMultipleSetups)
            {
                try
                {
                    actual.ShouldBeEquivalentTo(expected);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                actual.ShouldBeEquivalentTo(expected);

            return true;
        }
    }
}
