<p align="center">
<img src="uniscript.png" /><br>
Brings C# scripting into Unity which acts as native code.<br>
    <a href="docs/">Documentation</a> <b>/</b> <a href="https://pjc0247.github.io/UniScript/uniplayground/">Web Playground</a>
</p>

Overview
----
```cs
var src = @"
class PlayerMovement : MonoBehaviour {
    public void MoveForward() {
        transform.position += new Vector3(0, 0, 1);
    }
}
";

var script = CScript.CreateRunner(src);

dynamic move = script
    .Override("PlayerMovement", this)
    .AsDynamic();
move.MoveForward();
```

Yet Another C# Scripting Engine
---
Other C# scripts use `mcs` or `roslyn`. They're all compiler based not an interpreter<br>
however UniScript uses a __SlowSharp__ as a backend<br>
which enables....
* __Sandboxing__ : Can prevent malicious call with Whitelist, Blacklist or your own rules.
* __Fully compatible with iOS, WebAssembly and WSA__ : iOS is a huge market you can't abandon.
* __Execution timeout to prevent infinite loops__ : More safety on user created mods!

Supports Unity's native messages
----
Unity messages will be fired automatically, same as Native C#.
```cs
class MoveForward : UniScriptBehaviour { 
    public void Update() {
        transform.position += new Vector3(0, 0, 1);
    }
    public void OnEnable() { }
    public void OnDisable() { }
}
```
One only difference is all callbacks should be declared as public.

True Hot Reloading
----
Can replace methods after parsing. This also affects already instantiated objects. 
```cs
var r = CScript.CreateRunner(@"
class Foo { public int GiveMeNumber() => 10; }
");

var foo = r.Instantiate("Foo");
// should be 10
foo.Invoke("GiveMeNumber");
```
```cs
ss.UpdateMethodsOnly(@"
class Foo { public int GiveMeNumber() => 20; }
");
// should be 20
foo.Invoke("GiveMeNumber");
```


Runtime Debugging
----
<img src="debug.png" />

LICENSE
----
It doesn't have clear license at this moment, becuase this is very early stage of development and I'm not yet determined to sell this product or not.<br>
So just keep below lines.

* Non-Commercial/Commercial use are allowed.
* Sourcecode redistribution with chainging its name is not allowed.

However, __SlowSharp__ has its own license and you may publish code with some modifications.
