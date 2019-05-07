Unity IL2CPP code stripping
====

https://docs.unity3d.com/Manual/IL2CPP-BytecodeStripping.html

```xml
<linker>
  <assembly fullname="UnityEngine">
    <type fullname="UnityEngine.CharacterController"/>
    <type fullname="UnityEngine.Input"/>
    <type fullname="UnityEngine.Collider"/>
    <type fullname="UnityEngine.Collision"/>
    <type fullname="UnityEngine.Physics"/>
    <type fullname="UnityEngine.Transform"/>
    <type fullname="UnityEngine.GameObject"/>
    <type fullname="UnityEngine.WaitForSeconds"/>
  </assembly>
</linker>
```
