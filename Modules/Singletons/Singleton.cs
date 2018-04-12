/**
 * Singleton.cs
 * Created by: Joao Borks
 * Created on: 24/11/17 (dd/mm/yy)
 */

using UnityEngine;

/// <summary>
/// Base class for singleton types
/// </summary>
/// <typeparam name="T">Singleton class type</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// Active instance of the <see cref="T"/> singleton
    /// </summary>
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<T>();
                if (!instance)
                {
                    var type = typeof(T);
                    instance = new GameObject(type.Name, type).GetComponent<T>();
                }
            }
            return instance;
        }
    }
    /// <summary>
    /// Local reference to the active instance of the <see cref="T"/> singleton
    /// </summary>
    static T instance;

    /// <summary>
    /// Executed on instantiation. Ensures only one instance of the singleton exists and prevents destruction on scene load.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else if (!instance)
            instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}