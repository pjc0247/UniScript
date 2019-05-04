Working with AssetBundle
=====

Build
----
```cs
UniAssetBundle.BuildUniScriptScene(
  "testbundle_scene", 
  "testbundle_script");
```

You have to build create 2 seperated asset bundles since Unity does not allow to put scene and other assets ogether.

Load
----

```cs
UniAssetBundle.LoadUniScriptScene(
  "file:///Packs/testbundle",
  "file:///Packs/testbundle_script");
```
