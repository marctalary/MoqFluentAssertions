# MoqFluentAssertions
Combine Moq and Fluent Assertions for detailed testing feedback.

Usage:

```csharp
booMock.Verify(b => b.ItWorked(Its.EquivalentTo(barParam)));
```

Usage when equivalent check is between two different types:

```csharp
booMock.Verify(b => b.ItWorked(Its.EquivalentTo<BarParam, FooParam>(fooParam)));
```


