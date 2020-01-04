using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class with ready integration for the Save System framework
//Remove commented sections to enable it
public abstract class DevObjectBase : ScriptableObject/*, ISaveObject*/
{
    public abstract string[] GetCurrentValues();
    public abstract string[] GetKeys();
    public abstract void LoadSavedData(string[] values);

    public virtual void SetDefaultValues() { }
}
