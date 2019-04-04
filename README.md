<p align="center">
<h1>UniScript</h1><br>
<b>Brings C# scripting into Unity which works like native code.</b>
</p>


```cs
cs
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
