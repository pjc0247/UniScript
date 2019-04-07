Working with Editor referencing
====

Public field
----
```cs
public class DemoBehaviour : UniScriptBehaviour
{
    public Transform target;

    void Awake()
    {
        Bind(@"
using UnityEngine;

class DemoScript : DemoBehaviour {
    public void Update() {
        target.position += new Vector3(0, 0, 1);
    }
}");
    }
}
```

<img src="img/editor_ref_1.png" />


UnityEvent
----

```cs
public void OnClickButton() {
  // Bypass the click invocation to script
  instance.Invoke(nameof(OnClickButton));
}
```

<img src="img/editor_ref_2.png" />
