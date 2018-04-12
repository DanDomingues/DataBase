/**
 * SingletonRef.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 */

using UnityEngine;

/// <summary>
/// Base class for reference singleton types
/// </summary>
/// <typeparam name="T">Singleton class type</typeparam>
public abstract class SingletonRef<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// Active instance of the <see cref="T"/> reference singleton
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
                    Debug.LogErrorFormat("Reference Singleton of type {0} could not be found on the scene!", typeof(T).ToString());
                    return null;
                }
            }
            return instance;
        }
    }
    /// <summary>
    /// Local reference to the active instance of the <see cref="T"/> reference singleton
    /// </summary>
    static T instance;

    /// <summary>
    /// Executed on instantiation. Sets the reference to this singleton, if not already set
    /// </summary>
    protected virtual void Awake()
    {
        if (!instance)
            instance = GetComponent<T>();
    }
}