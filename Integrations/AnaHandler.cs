using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// Wrapper class for better usage of Unity's Analytics API
/// For further use, it is advised to extend the class and create specialized methods
/// </summary>

public static class AnaHandler
{

    /// <summary>
    /// Analytics custom event with custom key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="title"></param>
    /// <param name="value"></param>
    public static void CustomEvent(string key, string title, object value)
    {
        Analytics.CustomEvent(key, new Dictionary<string, object> { { title, value } });
    }

}
