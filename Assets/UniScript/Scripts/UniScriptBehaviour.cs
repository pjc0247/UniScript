using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slowsharp;

public class UniScriptBehaviour : MonoBehaviour
{
    public bool isBound => runner != null;
    public string src { get; private set; }

    protected Runner runner;
    protected HybInstance instance;

    private ScriptFlags flags;

    protected virtual void Awake()
    {
    }
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

    public void Bind(string src)
    {
        this.src = src;

        runner = CScript.CreateRunner(src);
        instance = runner.Override(
            GetBindableClass(), this);

        BuildFlags();

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
    private void BuildFlags()
    {
        flags.hasOnBind = HasMethod("OnBind");
        flags.hasUpdate = HasMethod(nameof(Update));
        flags.hasLateUpdate = HasMethod(nameof(LateUpdate));
        flags.hasOnDisable = HasMethod(nameof(OnDisable));
        flags.hasOnEnable = HasMethod(nameof(OnEnable));
        flags.hasOnMouseDown = HasMethod(nameof(OnMouseDown));
        flags.hasOnMouseUp = HasMethod(nameof(OnMouseUp));
        flags.hasOnDestroy = HasMethod(nameof(OnDestroy));
    }

    private bool HasMethod(string id)
        => instance.GetMethods(id).Length > 0;
}
