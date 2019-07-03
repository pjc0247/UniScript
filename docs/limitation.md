Limitation
====

Since __UniScript__ uses [SlowSharp](https://github.com/pjc0247/SlowSharp) as a backend, there're some differences compared to original __C#__.

await
----
__UniScript__ is designed for creating game flows not a core logic computation, Threading is not a major consideration at this moment.<br>
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

Instantiating generic classes from script is working, declaring generic classes or method is not supported yet.
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

Operator overloading
----
Currently, main purpose of __UniScript__ is writing a game flow using external texts not creating a datastructure.<br>
However, I'm totally agree with this is necessary.<br>
<br>
Will be implemented in near future.


Lambda
----
~~Will be implemented with high priority.~~ <br>
Now supports with partially implementation.<br>
https://github.com/pjc0247/SlowSharp/issues/1
 

And more...
----
* Latest language features such as `pattern matching` and `named tuple`.
* Special keywords: `unsafe`, `stackalloc`, `volatile` and so on.
