Limitation
====

await
----
Since __UniScript__ is designed for creating game flows not a core logic computation, Threading is not a major consideration.<br>
Current implementation of `await` keyword just __Waits__ until task ends and may blocks the execution thread.<br>
<br>
Here's a code from __SlowSharp__.
```cs
private HybInstance RunAwait(AwaitExpressionSyntax node) {
  var task = RunExpression(node.Expression);
  
  if (task.isCompiledType &&
      task.Unwrap() is Task t) {
      t.Wait();
      return HybInstance.Null();
  }
  /* ... And goes on */
}
```

goto
----
`goto` is very unstable for the moment. 

For example, every execution of `goto` creates a frame. this may lead program to `StackOverflowException` or memory leaks.
```cs
label:
Console.WriteLine("Hello World");
goto label;
```

Strongly not recommended to use.


generic
----
__generics__ are one of hardest part to implement. <br>

Instantiating generic classes from script is fine, declaring generic classes or method is not supported yet.
```cs
// This is OK
var list = new List<int>();

// Oops, Not implemented yet
class Foo<T> { }
```


partial
----
__SlowSharp__ is an interpreter and does not have `linking` phase during compliation. `partial` keyword won't be supported by design.

```cs
partial class Foo {
  public void A() { }
}
partial class Foo {
  public void B() { }
}
```
