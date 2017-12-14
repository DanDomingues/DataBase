/**
 * PoolMasterBase.cs
 * Created by: Joao Borks
 * Created on: 23/08/17 (dd/mm/yy)
 */

using UnityEngine;

/// <summary>
/// Base class for every Pool Master <see cref="Singleton{T}"/>
/// </summary>
/// <typeparam name="T">A <see cref="PoolMasterBase{T}"/> subclass</typeparam>
public abstract class PoolMasterBase<T> : Singleton<T> where T : Singleton<T>
{
    protected Transform poolParent;

    protected virtual void Awake()
    {
        var obj = new GameObject("Pool");
        obj.transform.SetParent(transform, false);
        obj.SetActive(false);
        poolParent = obj.transform;
        SetupPools();
    }

    /// <summary>
    /// Creates the <see cref="PoolSystem"/>s and fills the collections
    /// </summary>
    protected abstract void SetupPools();
}