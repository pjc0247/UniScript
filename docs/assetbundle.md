Working with AssetBundle
=====

Embedding a script into AssetBundle is not possible in Unity.<br>
With __UniScript__, you can embed `csx` scripts in your asset bundle.

Build
----
```cs
UniAssetBundle.BuildUniScriptScene(
  "testbundle_scene", 
  "testbundle_script");
```

You have to build create 2 seperated asset bundles because of Unity does not allow to put scene and other assets ogether.

Load
----

```cs
UniAssetBundle.LoadUniScriptScene(
  "file:///Packs/testbundle",
  "file:///Packs/testbundle_script");
```
