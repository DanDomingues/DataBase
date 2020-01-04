using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DevFieldBase : MonoBehaviour
{
    [SerializeField] protected string key;
    [SerializeField] protected string valueOutput;

    protected DevObjectEditor editor;
    protected bool ignoreInput;

    public string Key => key;
    public string Value => valueOutput;

    public virtual void SetEditor(DevObjectEditor value)
    {
        editor = value;
    }

    public virtual void SetKey(string key)
    {
        this.key = key;
    }

    public virtual void SetValue(string value)
    {
        ignoreInput = true;
        valueOutput = value;

        StartCoroutine(InvokeAfterFrame(() => ignoreInput = false));
    }
    IEnumerator InvokeAfterFrame(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        if (action != null) action();
    }

    public virtual void SendChanges()
    {
        if (ignoreInput) return;
        editor.GetChanges(key, valueOutput);
    }

}
