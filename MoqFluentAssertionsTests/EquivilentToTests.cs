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
            var expectedFooParam = new FooParam { Description = "tesdst description", ReferenceNumber = 123345 };
            foo.CallBoo(fooParam);

            // When
            var testException = Assert.Throws<XunitException>(() =>
                booMock.Verify(b => b.ItWorked(It.Is<BarParam>(o => o.ShouldBeEquivalentToReturnSuccess(expectedFooParam))))
            );

            // Then 
            var expectedStartOfException =
                $"Expected member {nameof(expectedFooParam.Description)} to be {Environment.NewLine}"
                + $"\"{expectedFooParam.Description}\" with a length of {expectedFooParam.Description.Length}, but {Environment.NewLine}"
                + $"\"{fooParam.Description}\" has a length of {fooParam.Description.Length}.{Environment.NewLine}"
                + $"Expected member {nameof(expectedFooParam.ReferenceNumber)} to be {expectedFooParam.ReferenceNumber}, "
                + $"but found {fooParam.ReferenceNumber}.";
            Assert.StartsWith(expectedStartOfException, testException.Message);
        }

        [Fact]
        public void Its_EquivalentTo_WithDiffereningTypes_DoesNotThrowException()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "test description", ReferenceNumber = 12345 };
            var expectedFooParam = new FooParam { Description = fooParam.Description, ReferenceNumber = fooParam.ReferenceNumber };
            foo.CallBoo(fooParam);

            // When
            booMock.Verify(b => b.ItWorked(Its.EquivalentTo<BarParam, FooParam>(expectedFooParam)));

            // Then 
            // No Exception thrown
        }

        [Fact]
        public void Its_EquivalentTo_WithDiffereningTypes_ThrowsAssertionExcpetion()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "actual description", ReferenceNumber = 12345 };
            var expectedFooParam = new FooParam { Description = "expected description", ReferenceNumber = 54321 };
            foo.CallBoo(fooParam);

            // When
            var testException = Assert.Throws<XunitException>(() =>
                booMock.Verify(b => b.ItWorked(Its.EquivalentTo<BarParam, FooParam>(expectedFooParam)))
            );

            // Then 
            var expectedStartOfException =
                $"Expected member {nameof(expectedFooParam.Description)} to be {Environment.NewLine}\"{expectedFooParam.Description}\" with a length of {expectedFooParam.Description.Length}, "
                + $"but {Environment.NewLine}\"{fooParam.Description}\" has a length of {fooParam.Description.Length}."
                + $"{Environment.NewLine}Expected member {nameof(expectedFooParam.ReferenceNumber)} to be {expectedFooParam.ReferenceNumber}, "
                + $"but found {fooParam.ReferenceNumber}.";
            Assert.StartsWith(expectedStartOfException, testException.Message);
        }

        [Fact]
        public void Its_EquivalentTo_DoesNotThrowException()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "test description", ReferenceNumber = 12345 };
            var expectedBarParam = new BarParam { Description = fooParam.Description, ReferenceNumber = fooParam.ReferenceNumber };
            foo.CallBoo(fooParam);

            // When
            booMock.Verify(b => b.ItWorked(Its.EquivalentTo(expectedBarParam)));

            // Then 
            // No Exception thrown
        }

        [Fact]
        public void Its_EquivalentTo_ThrowsAssertionExcpetion()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var expectedBarParam = new BarParam { Description = "expected description", ReferenceNumber = 12345 };
            var barParam = new BarParam { Description = "actual description", ReferenceNumber = 54321 };
            foo.CallBoo(barParam);

            // When
            var testException = Assert.Throws<XunitException>(() =>
                booMock.Verify(b => b.ItWorked(Its.EquivalentTo(expectedBarParam)))
            );

            // Then 
            var expectedStartOfException =
                $"Expected member {nameof(expectedBarParam.Description)} to be {Environment.NewLine}\"{expectedBarParam.Description}\" with a length of {expectedBarParam.Description.Length}, "
                + $"but {Environment.NewLine}\"{barParam.Description}\" has a length of {barParam.Description.Length}."
                + $"{Environment.NewLine}Expected member {nameof(expectedBarParam.ReferenceNumber)} to be {expectedBarParam.ReferenceNumber}, "
                + $"but found {barParam.ReferenceNumber}.";

            Assert.StartsWith(expectedStartOfException, testException.Message);
        }

        [Fact]
        public void Its_EquivalentTo_DoesNotThrowException_WithMultipleSetups()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "test description", ReferenceNumber = 12345 };
            var expectedBarParam = new BarParam { Description = fooParam.Description, ReferenceNumber = fooParam.ReferenceNumber };

            booMock.Setup(b => b.ItWorked(Its.EquivalentTo(expectedBarParam, true)));
            booMock.Setup(b => b.ItWorked(Its.EquivalentTo(new BarParam(), true)));
            foo.CallBoo(fooParam);
            foo.CallBoo(new FooParam());

            // When
            booMock.Verify(b => b.ItWorked(Its.EquivalentTo(expectedBarParam, true)), Times.Once);
            booMock.Verify(b => b.ItWorked(Its.EquivalentTo(new BarParam(), true)), Times.Once);

            // Then 
            // No Exception thrown
        }

        [Fact]
        public void Its_EquivalentTo_ThrowsException_WithMultipleSetups()
        {
            // Given
            var booMock = new Mock<IBar>();
            var foo = new Foo(booMock.Object);
            var fooParam = new FooParam { Description = "given value 1", ReferenceNumber = 12345 };
            var fooParam2 = new FooParam { Description = "given value 2", ReferenceNumber = 23456 };
            var expectedBarParam = new BarParam { Description = fooParam.Description, ReferenceNumber = fooParam.ReferenceNumber };
            var expectedBarParam2 = new BarParam { Description = fooParam2.Description, ReferenceNumber = fooParam2.ReferenceNumber };
            var unExpectedBarParam = new BarParam { Description = "Unexpected value" };

            booMock.Setup(b => b.ItWorked(Its.EquivalentTo(expectedBarParam, true)));
            booMock.Setup(b => b.ItWorked(Its.EquivalentTo(expectedBarParam2, true)));
            foo.CallBoo(fooParam);
            foo.CallBoo(fooParam2);

            // When
            booMock.Verify(b => b.ItWorked(Its.EquivalentTo(expectedBarParam, true)), Times.Once);
            booMock.Verify(b => b.ItWorked(Its.EquivalentTo(expectedBarParam2, true)), Times.Once);

            // and When Then
            var testException = Assert.Throws<MockException>(() =>
                booMock.Verify(b => b.ItWorked(Its.EquivalentTo(unExpectedBarParam, true)), Times.Once)
            );

            Assert.StartsWith("Expected invocation on the mock once, but was 0 times", testException.Message.Trim());
        }
    }
}