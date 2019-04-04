<p align="center">
<b>UniScript</b><br>
Brings C# scripting into Unity which acts as native code.
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
Other C# scripts use `mcs` or `roslyn`. They're all compiler based not a interpreter<br>
however `UniScript` uses a __SlowSharp__ as a backend<br>
which enables....
* __Sandboxing__
* __Fully compatible with iOS, WebAssembly and WSA__
* __Execution timeout to prevent infinite loops__
