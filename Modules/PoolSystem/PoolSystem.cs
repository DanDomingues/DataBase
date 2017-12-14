/**
 * PoolSystem.cs
 * Created by: Joao Borks [joao.borks@gmail.com]
 * Created on: 12/07/17 (dd/mm/yy)
 */

using UnityEngine;

/// <summary>
/// Structure to manage any pool system based on prefabs
/// </summary>
public class PoolSystem
{
    /// <summary>
    /// <see cref="Transform"/> object to hold all <see cref="PoolSystem"/> dynamic active objects
    /// </summary>
    static Transform dynamicMaster;

    /// <summary>
    /// List of all pooled objects in this <see cref="PoolSystem"/>
    /// </summary>
    GameObject[] pooledObjects;
    /// <summary>
    /// Prefab to be instantiated then pooled
    /// </summary>
    GameObject poolPrefab;
    /// <summary>
    /// Parent of this <see cref="PoolSystem"/>'s dynamic active objects
    /// </summary>
    Transform dynamicParent;
    /// <summary>
    /// Parent of this <see cref="PoolSystem"/>'s pooled inactive objects
    /// </summary>
    Transform pooledParent;
    /// <summary>
    /// Total amount of objects on this <see cref="PoolSystem"/>
    /// </summary>
    int poolSize;
    /// <summary>
    /// Current access index of the <see cref="pooledObjects"/> array
    /// </summary>
    int curIndex;

    /// <summary>
    /// Creates a new <see cref="PoolSystem"/>
    /// </summary>
    /// <param name="poolPrefab">The prefab <see cref="GameObject"/> to be created. Must implement <see cref="IPoolObject"/></param>
    /// <param name="pooledParent">Parent of this <see cref="PoolSystem"/>'s pooled inactive objects. Must be inactive in hierarchy</param>
    /// <param name="poolSize">Total amount of objects on this <see cref="PoolSystem"/></param>
    /// <param name="dynamicSection">Name of the object that will store this <see cref="PoolSystem"/>'s dynamic active objects
    /// under the general <see cref="dynamicMaster"/> object</param>
    public PoolSystem(GameObject poolPrefab, Transform pooledParent, int poolSize, string dynamicSection)
    {
        if (!poolPrefab || poolPrefab.GetComponent<IPoolObject>() == null)
            throw new System.ArgumentException("Pool Prefab must be a valid GameObject and implement IPoolObject!", "poolPrefab");

        this.poolPrefab = poolPrefab;

        if (!dynamicMaster)
            dynamicMaster = new GameObject("DynamicPoolObjects").transform;

        if (string.IsNullOrEmpty(dynamicSection))
            dynamicParent = dynamicMaster;
        else
        {
            var existing = dynamicMaster.Find(dynamicSection);
            if (existing)
                dynamicParent = existing;
            else
            {
                dynamicParent = new GameObject(dynamicSection).transform;
                dynamicParent.SetParent(dynamicMaster, false);
            }
        }

        if (!pooledParent || pooledParent.gameObject.activeInHierarchy)
            throw new System.ArgumentException("Pool Parent must be a valid Transform and inactive in hierarchy!", "pooledParent");

        this.pooledParent = pooledParent;

        if (poolSize <= 0)
            throw new System.ArgumentException("Pool size must be greater than 0!", "poolSize");

        this.poolSize = poolSize;
        pooledObjects = new GameObject[poolSize];
        InitializePool();
        curIndex = poolSize - 1;
    }

    /// <summary>
    /// Recycles the given <paramref name="obj"/> adding it back to the <see cref="pooledObjects"/> list
    /// </summary>
    /// <param name="obj">Object to be added back into the pool</param>
    public void DisposeObject(GameObject obj)
    {
        var poolComponent = obj.GetComponent<IPoolObject>();
        if (poolComponent == null || !Equals(poolComponent.Pool))
            throw new System.ArgumentException("Object must implement IPoolObject and be a child of this Pool System!", "obj");
        if (curIndex == poolSize - 1)
            throw new System.Exception("Pool has no empty spaces. Make sure not to dispose the same object twice");
        obj.transform.SetParent(pooledParent, false);
        curIndex++;
        pooledObjects[curIndex] = obj;
    }

    /// <summary>
    /// Gets an object from the pool, removing it from the <see cref="PoolSystem"/> and activating it on the hierarchy
    /// </summary>
    public GameObject GetObject()
    {
		if (curIndex < 0)
            throw new System.Exception("No more available objects on the pool out of " + poolSize + " total, try increasing the maximum pool size");
        GameObject obj;
        obj = pooledObjects[curIndex];
        pooledObjects[curIndex] = null;
        curIndex--;
        obj.transform.SetParent(dynamicParent, false);
        return obj;
    }

    /// <summary>
    /// Instantiate and store all pool objects on the <see cref="pooledObjects"/> list
    /// </summary>
    void InitializePool()
    {
        GameObject obj;
        for (int i = 0; i < poolSize; i++)
        {
            obj = Object.Instantiate(poolPrefab, pooledParent);
            obj.name = poolPrefab.name;
            obj.GetComponent<IPoolObject>().Pool = this;
            pooledObjects[i] = obj;
        }
    }
}