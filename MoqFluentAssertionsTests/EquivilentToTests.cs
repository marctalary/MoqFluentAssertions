using System;
using FluentAssertions;
using Moq;
using MoqFluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace MoqFluentAssertionsTests
{
    public class EquivilentToTests
    {
        public class Foo
        {
            private readonly IBar _bar;

            public Foo(IBar bar)
            {
                _bar = bar;
            }

            public void CallBoo(FooParam param)
            {
                _bar.ItWorked(new BarParam { Description = param.Description, ReferenceNumber = param.ReferenceNumber });
            }

            public void CallBoo(BarParam param)
            {
                _bar.ItWorked(new BarParam { Description = param.Description, ReferenceNumber = param.ReferenceNumber });
            }
        }

        public class FooParam
        {
            public string Description { get; set; }
            public int ReferenceNumber { get; set; }
        }

        public class BarParam
        {
            public string Description { get; set; }
            public int ReferenceNumber { get; set; }
        }

        public interface IBar
        {
            string ItWorked(BarParam param);
        }


        [Fact]
        public void ShouldBeEquivalentToReturnSuccess()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "test description", ReferenceNumber = 12345 };

            // When
            foo.CallBoo(fooParam);

            // Then 
            booMock.Verify(b => b.ItWorked(It.Is<BarParam>(o => o.ShouldBeEquivalentToReturnSuccess(fooParam))));
        }

        [Fact]
        public void ShouldBeEquivalentToReturnSuccess_FluentAssertFail()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "test description", ReferenceNumber = 12345 };
            var fooParamMisMatch = new FooParam { Description = "tesdst description", ReferenceNumber = 123345 };
            foo.CallBoo(fooParam);

            // When
            var testException = Assert.Throws<XunitException>(() =>
                booMock.Verify(b => b.ItWorked(It.Is<BarParam>(o => o.ShouldBeEquivalentToReturnSuccess(fooParamMisMatch))))
            );

            // Then 
            var expectedStartOfException =
                $"Expected member {nameof(fooParam.Description)} to be {Environment.NewLine}"
                + $"\"{fooParam.Description}\" with a length of {fooParam.Description.Length}, but {Environment.NewLine}"
                + $"\"{fooParamMisMatch.Description}\" has a length of {fooParamMisMatch.Description.Length}.{Environment.NewLine}"
                + $"Expected member {nameof(fooParam.ReferenceNumber)} to be {fooParam.ReferenceNumber}, "
                + $"but found {fooParamMisMatch.ReferenceNumber}.";
            Assert.StartsWith(expectedStartOfException, testException.Message);
        }

        [Fact]
        public void Its_EquivalentTo_DoesNotThrowException()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "test description", ReferenceNumber = 12345 };
            foo.CallBoo(fooParam);

            // When
            booMock.Verify(b => b.ItWorked(Its.EquivalentTo<BarParam, FooParam>(fooParam)));

            // Then 
            // No Exception thrown
        }

        [Fact]
        public void Its_EquivalentTo_WithDiffereningTypes_ThrowsAssertionExcpetion()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "test description", ReferenceNumber = 12345 };
            var fooParamMisMatch = new FooParam { Description = "tesdst description", ReferenceNumber = 123345 };
            foo.CallBoo(fooParam);

            // When
            var testException = Assert.Throws<XunitException>(() =>
                booMock.Verify(b => b.ItWorked(Its.EquivalentTo<BarParam, FooParam>(fooParamMisMatch)))
            );

            // Then 
            var expectedStartOfException =
                $"Expected member {nameof(fooParam.Description)} to be {Environment.NewLine}\"{fooParam.Description}\" with a length of {fooParam.Description.Length}, "
                + $"but {Environment.NewLine}\"{fooParamMisMatch.Description}\" has a length of {fooParamMisMatch.Description.Length}."
                + $"{Environment.NewLine}Expected member {nameof(fooParam.ReferenceNumber)} to be {fooParam.ReferenceNumber}, "
                + $"but found {fooParamMisMatch.ReferenceNumber}.";
            Assert.StartsWith(expectedStartOfException, testException.Message);
        }

        [Fact]
        public void Its_EquivalentTo_ThrowsAssertionExcpetion()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var barParam = new BarParam { Description = "test description", ReferenceNumber = 12345 };
            var barParamMisMatch = new BarParam { Description = "tesdst description", ReferenceNumber = 123345 };
            foo.CallBoo(barParam);

            // When
            var testException = Assert.Throws<XunitException>(() =>
                booMock.Verify(b => b.ItWorked(Its.EquivalentTo(barParamMisMatch)))
            );

            // Then 
            var expectedStartOfException =
                $"Expected member {nameof(barParam.Description)} to be {Environment.NewLine}\"{barParam.Description}\" with a length of {barParam.Description.Length}, "
                + $"but {Environment.NewLine}\"{barParamMisMatch.Description}\" has a length of {barParamMisMatch.Description.Length}."
                + $"{Environment.NewLine}Expected member {nameof(barParam.ReferenceNumber)} to be {barParam.ReferenceNumber}, "
                + $"but found {barParamMisMatch.ReferenceNumber}.";

            Assert.StartsWith(expectedStartOfException, testException.Message);
        }
    }
}