# MoqFluentAssertions
Combine [Moq](https://github.com/moq/moq) and [Fluent Assertions](https://github.com/fluentassertions/fluentassertions) for detailed testing feedback and comparison capabilities.

## Its.EquivalentTo

Usage:

```csharp
booMock.Verify(b => b.ItWorked(Its.EquivalentTo(barParam)));
```

Usage when equivalent check is between two different types:

```csharp
booMock.Verify(b => b.ItWorked(Its.EquivalentTo<BarParam, FooParam>(fooParam)));
```

This gives rich feedback on failure courtesy of the `FluentAssertions.ShouldBeEquivalentTo` method used to implement it.

> Expected member Description to be <br>
> "expected description" with a length of 20, but <br>
> "actual description" has a length of 18.<br>
> Expected member ReferenceNumber to be 12345, but found 54321.<br>

However using this method has major drawbacks.  It works by throwing an ```Exception``` with the test message and so fails the test whenever it is given an object that isn't equivalent, even when used in a ```Moq.Setup```.  

By contrast the behavior of the ```Moq.It.Is()``` method takes an expression which returns true or false for each object given and then, if they are all false the ```Moq.Verify``` fails the test by throwing a ```MockExpcetion``` or when used in a ```Moq.Setup``` statement does not evoke the statements expression usually a ```Return```.

Therefore it is never advisable to use `Its.EquivalentTo` in setups and only useful in ```Moq.Verify``` when you only have a single verify for the method signature. 

To get around this there is an ```AllowMultipleSetups``` parameter.

### AllowMultipleSetups

Usage:

```csharp
booMock.Verify(b => b.ItWorked(Its.EquivalentTo(barParam, true)));
```

Usage when equivalent check is between two different types:

```csharp
booMock.Verify(b => b.ItWorked(Its.EquivalentTo<BarParam, FooParam>(fooParam, true)));
```

The  `AllowMultipleSetups` parameter defaults to ```false``` but when ```true``` does the ```FluentAssertions.ShouldBeEquivalentTo``` comparison but instead of failing the test on first non-equivalent object implements the same behavior as ```Moq.It.Is()```.   

You lose the rich feedback from `FluentAssertions` but gain the capability of using `Its.EquivalentTo` anywhere you would have used `Moq.It.Is()`.

In the example below the test would have failed on the `foo.CallBoo(fooParam);` statement as all of the ```Moq.setup``` statements are checked when applicable and the second `Its.EquivilentTo` would have ended the test. 

By passing `allowMultipleSetups: true` the call will only fail if there are no relevant `Moq.Setup` statements.

```c#
var fooParam = new FooParam
{
    Description = "given value 1", ReferenceNumber = 12345
};
var fooParam2 = new FooParam
{
    Description = "given value 2", ReferenceNumber = 23456
};
var expectedBarParam = new BarParam
{
    Description = fooParam.Description, ReferenceNumber = fooParam.ReferenceNumber
};
var expectedBarParam2 = new BarParam
{
    Description = fooParam2.Description, ReferenceNumber = fooParam2.ReferenceNumber
};

booMock.Setup(b => b.ItWorked(Its.EquivalentTo(expectedBarParam, allowMultipleSetups: true)));
booMock.Setup(b => b.ItWorked(Its.EquivalentTo(expectedBarParam2, allowMultipleSetups: true)));
foo.CallBoo(fooParam);
foo.CallBoo(fooParam2);
```

