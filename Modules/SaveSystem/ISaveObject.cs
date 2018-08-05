using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveObject
{
    /// <summary>
    /// Gets keys for saving and loading data
    /// </summary>
    string[] GetKeys();
    /// <summary>
    /// Gets the current values from the object
    /// </summary>
    /// <returns></returns>
    string[] GetCurrentValues();
    /// <summary>
    /// Gets the saved values and applies to the object
    /// </summary>
    void LoadSavedData();
    /// <summary>
    /// Sets the default values used when there is no saved file, or it has been overwritten
    /// </summary>
    void SetDefaultValues();
}
