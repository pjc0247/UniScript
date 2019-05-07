using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Slowsharp;

using UniScript.Serialization;

public class UniScriptBehaviour : MonoBehaviour
{
    public bool isBound => runner != null;
    public string src { get; private set; }

    [HideInInspector]
    public SerializableDictionarySO overrideFields;

    protected CScript runner;
    protected HybInstance instance;

    private ScriptFlags flags;

    void Update()
    {
        if (flags.hasUpdate)
            instance.Invoke(nameof(Update));
    }
    void LateUpdate()
    {
        if (flags.hasLateUpdate)
            instance.Invoke(nameof(LateUpdate));
    }
    void OnEnable()
    {
        if (flags.hasOnEnable)
            instance.Invoke(nameof(OnEnable));
    }
    void OnDisable()
    {
        if (flags.hasOnDisable)
            instance.Invoke(nameof(OnDisable));
    }
    void OnDestroy()
    {
        if (flags.hasOnDestroy)
            instance.Invoke(nameof(OnDestroy));
    }
    void OnMouseDown()
    {
        if (flags.hasOnMouseDown)
            instance.Invoke(nameof(OnMouseDown));
    }
    void OnMouseUp()
    {
        if (flags.hasOnMouseUp)
            instance.Invoke(nameof(OnMouseUp));
    }

    void OnTriggerEnter(Collider other)
    {
        if (flags.hasOnTriggerEnter)
            instance.Invoke(nameof(OnTriggerEnter), other);
    }
    void OnTriggerExit(Collider other)
    {
        if (flags.hasOnTriggerExit)
            instance.Invoke(nameof(OnTriggerExit), other);
    }
    void OnCollisionEnter(Collision other)
    {
        if (flags.hasOnCollisionEnter)
            instance.Invoke(nameof(OnCollisionEnter), other);
    }
    void OnCollisionExit(Collision other)
    {
        if (flags.hasOnCollisionExit)
            instance.Invoke(nameof(OnCollisionExit), other);
    }
    /*
    public GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }
    */

    public void Bind(string src)
    {
        this.src = src;

        var scriptConfig = ScriptConfig.Default;
        scriptConfig.PrewarmTypes = new Type[] {
            typeof(Collider), typeof(Transform), typeof(Vector3),
            typeof(Vector2), typeof(Vector4), typeof(GameObject)
        };

        runner = CScript.CreateRunner(src, scriptConfig);
        RuntimeScript.runner = runner;
        instance = runner.Override(
            GetBindableClass(), this);

        BuildFlags();

        if (overrideFields != null)
        {
            foreach (var f in overrideFields)
                instance.SetPropertyOrField(f.Key, HybInstance.Object(f.Value));
            overrideFields = null;
        }

        if (flags.hasOnBind)
            instance.Invoke("OnBind");
    }
    private string GetBindableClass()
    {
        foreach (var type in runner.GetTypes())
        {
            if (type.parent != null &&
                type.parent.isCompiledType &&
                type.parent.compiledType == GetType())
            {
                return type.id;
            }
        }
        throw new ArgumentException(
            $"Script does not contains class that overrides {GetType()}.");
    }
    protected virtual void BuildFlags()
    {
        flags.hasOnBind = HasMethod("OnBind");
        flags.hasUpdate = HasMethod(nameof(Update));
        flags.hasLateUpdate = HasMethod(nameof(LateUpdate));
        flags.hasOnDisable = HasMethod(nameof(OnDisable));
        flags.hasOnEnable = HasMethod(nameof(OnEnable));
        flags.hasOnMouseDown = HasMethod(nameof(OnMouseDown));
        flags.hasOnMouseUp = HasMethod(nameof(OnMouseUp));
        flags.hasOnDestroy = HasMethod(nameof(OnDestroy));
        flags.hasOnTriggerEnter = HasMethod(nameof(OnTriggerEnter));
        flags.hasOnTriggerExit = HasMethod(nameof(OnTriggerExit));
        flags.hasOnCollisionEnter = HasMethod(nameof(OnCollisionEnter));
        flags.hasOnCollisionExit = HasMethod(nameof(OnCollisionExit));
    }

    protected bool HasMethod(string id)
        => instance.GetMethods(id).Length > 0;
}
