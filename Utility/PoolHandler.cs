using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    public static PoolHandler instance;

    public Transform poolHost;
    public Transform objHost;

    public List<PoolData> pools;

#if UNITY_EDITOR
    [Header("Editor")]
    [SerializeField]
    GameObject[] objectsToAdd;

#endif

    private void Awake()
    {
        instance = this;

        if (pools == null) pools = new List<PoolData>();
        pools = pools.Where(pool => pool.prefab != null).ToList();

        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].bank.gameObject.SetActive(false);
        }

    }

    public void SetHost(Transform host)
    {
        objHost = host;
    }

    public void AddPool(GameObject prefab, Transform objParent)
    {
        PoolData pool = new PoolData(prefab, poolHost, objParent);
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].prefab == prefab)
            {
                Debug.Log("Object pool already exists");
                return;
            }
        }

        pools.Add(pool);
    }

    public void AddPool(GameObject prefab)
    {
        AddPool(prefab, null);
    }

    public void AddPool(GameObject[] prefabs)
    {
        AddPool(prefabs, null);
    }

    public void AddPool(GameObject[] prefabs, Transform parent)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            AddPool(prefabs[i], parent);
        }

    }

    public GameObject GetObject(string key)
    {
        GameObject value = null;
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].key == key)
            {
                value = pools[i].GetObject();
                return value;
            }
        }

        return value;
    }

    public GameObject GetObject(string key, Vector3 position)
    {
        GameObject value = null;
        for (int i = 0; i < pools.Count; i++)
        {
            if(pools[i].key == key)
            {
                value = pools[i].GetObject();
                value.transform.localPosition = position;
                return value;
            }
        }

        return value;
    }

    public GameObject GetObject(string key, float returnTime)
    {
        GameObject value = GetObject(key);
        if (value != null)
        {
            StoreObject(value, returnTime);
        }

        return value;
    }

    public GameObject GetObject(string key, Vector3 position, float returnTime)
    {
        GameObject value = GetObject(key, position);
        if (value != null)
        {
            StoreObject(value, returnTime);
        }

        return value;
    }

    public void StoreObject(GameObject input)
    {
        int index = -1;
        string reference = "";

        for (int i = 0; i < pools.Count; i++)
        {
            if(input.name.Contains(pools[i].key) && pools[i].key.Length > reference.Length)
            {
                index = i;
                reference = pools[i].key;
            }
        }

        if(index >= 0) pools[index].StoreObject(input);
    }

    public void StoreObject(GameObject input, float delay)
    {
        InvokeAlternatives.Invoke(this, delay, () => { StoreObject(input); });
    }

#if UNITY_EDITOR
    [ContextMenu("Add Editor Objects as Pools")]
    public void AddObjectsToPools()
    {
        AddPool(objectsToAdd);
    }

#endif

    [System.Serializable]
    public struct PoolData
    {
        public string key;
        public Transform bank;
        public Transform actionParent;
        public GameObject prefab;

        public PoolData(GameObject prefab, Transform poolParent, Transform objParent)
        {
            this.prefab = prefab;
            key = prefab.name;
            actionParent = objParent;

            bank = new GameObject(key + "Pool").transform;
            bank.parent = poolParent;
            bank.transform.position = Vector3.zero;

            bank.gameObject.SetActive(false);
        }

        public GameObject GetObject()
        {
            return ObjSetup();
        }

        public GameObject GetObject(Vector3 spawnLocalPos)
        {
            GameObject obj = ObjSetup();
            obj.transform.localPosition = spawnLocalPos;

            return obj;
        }

        private GameObject ObjSetup()
        {
            GameObject value = null;
            if (bank.childCount > 0)
            {
                value = bank.GetChild(0).gameObject;
            }
            else
            {
                value = Instantiate(prefab);
            }
          
            value.name = key;
            value.transform.SetParent(actionParent);

            return value;

        }

        public void StoreObject(GameObject obj)
        {
            obj.transform.SetParent(bank);
        }
    }
}
